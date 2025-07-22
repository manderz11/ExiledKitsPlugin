using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;

namespace ExiledKitsPlugin.Classes;

public class KitManager
{
    public List<CooldownEntry> CooldownEntries = new List<CooldownEntry>();
    public List<KitUseEntry> KitUseEntries = new List<KitUseEntry>();
    // just a slight optimisation if there aren't many entries with an initial cooldown
    public List<KitEntry> InitialCooldownKitEntries = new List<KitEntry>();
    public Dictionary<Player, double> PlayerSpawnTime = new Dictionary<Player, double>();

    public KitManager()
    {
        CooldownEntries = new List<CooldownEntry>();
        InitialCooldownKitEntries = new List<KitEntry>();
        KitUseEntries = new List<KitUseEntry>();
        foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
        {
            if (kitEntry.InitialCooldown > 0f)
            {
                InitialCooldownKitEntries.Add(kitEntry);
            }
        }
    }
    
    public void ResetKitCooldowns()
    {
        if (Plugin.Instance.Config.ResetKitCooldownsOnRoundRestart)
        {
            foreach (var cooldownEntry in CooldownEntries.ToList())
            {
                if (cooldownEntry.Kit.ResetCooldownOnRoundRestart != false)
                {
                    CooldownEntries.Remove(cooldownEntry);
                }
            }
            foreach (var kitEntry in InitialCooldownKitEntries.ToList())
            {
                if (kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    InitialCooldownKitEntries.Remove(kitEntry);
                }
            }

            foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
            {
                if (kitEntry.InitialCooldown > 0f && kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    Plugin.Instance.KitManager.InitialCooldownKitEntries.Add(kitEntry);
                }
                
            }
        }
        else
        {
            foreach (var cooldownEntry in CooldownEntries.ToList())
            {
                if (cooldownEntry.Kit.ResetCooldownOnRoundRestart == true)
                {
                    CooldownEntries.Remove(cooldownEntry);
                }
            }
            foreach (var kitEntry in InitialCooldownKitEntries.ToList())
            {
                if (kitEntry.ResetCooldownOnRoundRestart == true)
                {
                    InitialCooldownKitEntries.Remove(kitEntry);
                }
            }

            foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
            {
                if (kitEntry.InitialCooldown > 0f && kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    Plugin.Instance.KitManager.InitialCooldownKitEntries.Add(kitEntry);
                }
                
            }
        }
    }

    public void ResetKitUses()
    {
        if (Plugin.Instance.Config.ResetKitUsesOnRoundRestart)
        {
            foreach (var kitUseEntry in KitUseEntries.ToList())
            {
                if (kitUseEntry.KitEntry.ResetUsesOnRoundRestart != false)
                {
                    KitUseEntries.Remove(kitUseEntry);
                }
            }
        }
        else
        {
            foreach (var kitUseEntry in KitUseEntries.ToList())
            {
                if (kitUseEntry.KitEntry.ResetUsesOnRoundRestart == true)
                {
                    KitUseEntries.Remove(kitUseEntry);
                }
            }
        }
    }

    public bool IsKitEntryOnCooldown(KitEntry kitEntry, Player player)
    {
        if (CooldownEntries == null)
        {
            return false;
        }

        CooldownEntry cooldownEntry = CooldownEntries.Find(x => x.Kit == kitEntry && x.Player.UserId == player.UserId);
        if (cooldownEntry == null)
        {
            return false;
        }

        if (cooldownEntry.RemainingTime <= 0)
        {
            CooldownEntries.Remove(cooldownEntry);
            return false;
        }
        
        return true;
    }

    public void StartKitCooldown(KitEntry kitEntry, Player player, float time)
    {
        CooldownEntry cooldownEntry = GetCooldownEntry(player, kitEntry);
        if (cooldownEntry == null)
        {
            CooldownEntry kitCooldownEntry = new CooldownEntry(player, kitEntry, time);
            CooldownEntries.Add(kitCooldownEntry);
            return;
        }

        if (cooldownEntry.RemainingTime <= 0)
        {
            CooldownEntries.Remove(cooldownEntry);
            CooldownEntry kitCooldownEntry = new CooldownEntry(player, kitEntry, time);
            CooldownEntries.Add(kitCooldownEntry);
        }
    }

    public float GetTimeLeft(KitEntry kitEntry, Player player)
    {
        if (CooldownEntries == null)
        {
            return -1f;
        }

        if (!IsKitEntryOnCooldown(kitEntry,player))
        {
            return -1f;
        }

        CooldownEntry cooldownEntry = CooldownEntries.Find(x => x.Player.UserId == player.UserId && x.Kit == kitEntry);
        return cooldownEntry.RemainingTime;
    }

    public CooldownEntry GetCooldownEntry(Player player, KitEntry kitEntry)
    {
        CooldownEntry cooldownEntry = CooldownEntries.Find(x => x.Player.UserId == player.UserId && x.Kit == kitEntry);
        return cooldownEntry;
    }

    public KitUseEntry GetKitUseEntry(Player player, KitEntry kitEntry)
    {
        KitUseEntry kitUseEntry = KitUseEntries.Find(x => x.Player.UserId == player.UserId && x.KitEntry == kitEntry);
        return kitUseEntry;
    }

    public void RemovePlayerEntryData(Player player)
    {
        /*
        foreach (var playerCooldownEntry in CooldownEntries.FindAll(x => x.Player == player))
        {
            CooldownEntries.Remove(playerCooldownEntry);
        }

        foreach (var playerKitUseEntry in KitUseEntries.FindAll(x => x.Player == player))
        {
            KitUseEntries.Remove(playerKitUseEntry);
        }
        */

        if (PlayerSpawnTime.ContainsKey(player))
        {
            PlayerSpawnTime.Remove(player);
        }
    }
}

public class CooldownEntry
{
    public CooldownEntry(Player player, KitEntry kitEntry, float cooldownDuration)
    {
        Player = player;
        Kit = kitEntry;
        UserID = player.UserId;
        RemainingTime = cooldownDuration;
        Timing.RunCoroutine(ReduceTime());
    }

    public CooldownEntry(Player player)
    {
        Player = player;
        UserID = player.UserId;
    }

    public CooldownEntry(Player player, KitEntry kitEntry)
    {
        Player = player;
        Kit = kitEntry;
        UserID = player.UserId;
    }

    public KitEntry Kit;
    public Player Player;
    public string UserID;
    public float RemainingTime { get; private set; }

    public IEnumerator<float> ReduceTime()
    {
        while (RemainingTime > 0f)
        {
            RemainingTime -= 1f;
            yield return Timing.WaitForSeconds(1);
        }
    }
}

public class KitUseEntry
{
    public KitUseEntry(KitEntry kitEntry, Player player, int uses)
    {
        Player = player;
        KitEntry = kitEntry;
        UserID = player.UserId;
        Uses = uses;
    }

    public KitUseEntry(KitEntry kitEntry, Player player)
    {
        Player = player;
        KitEntry = kitEntry;
        UserID = player.UserId;
    }

    public KitEntry KitEntry;
    public Player Player;
    public string UserID;
    public int Uses;
}