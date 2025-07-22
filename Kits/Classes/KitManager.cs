using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace ExiledKitsPlugin.Classes;

public class KitManager
{
    public List<CooldownEntry> CooldownEntries = new List<CooldownEntry>();
    public List<KitUseEntry> KitUseEntries = new List<KitUseEntry>();
    // just a slight optimisation if there aren't many entries with an initial cooldown
    public List<KitEntry> InitialCooldownKitEntries = new List<KitEntry>();
    public List<KitEntry> TimeoutKitEntries = new List<KitEntry>();
    public List<KitEntry> InitialGlobalCooldownKitEntries = new List<KitEntry>();
    public Dictionary<Player, double> PlayerSpawnTime = new Dictionary<Player, double>();

    public KitManager()
    {
        CooldownEntries = new List<CooldownEntry>();
        InitialCooldownKitEntries = new List<KitEntry>();
        TimeoutKitEntries = new List<KitEntry>();
        InitialGlobalCooldownKitEntries = new List<KitEntry>();
        KitUseEntries = new List<KitUseEntry>();
        foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
        {
            if (kitEntry.InitialCooldown > 0f)
            {
                InitialCooldownKitEntries.Add(kitEntry);
            }

            if (kitEntry.GlobalKitTimeout > 0f)
            {
                TimeoutKitEntries.Add(kitEntry);
            }

            if (kitEntry.InitialGlobalCooldown > 0f)
            {
                InitialGlobalCooldownKitEntries.Add(kitEntry);
            }
        }
    }
    
    public void ResetKitCooldowns()
    {
        List<CooldownEntry> cooldownEntriesToRemove = new List<CooldownEntry>();
        List<KitEntry> initialCooldownKitEntriesToRemove = new List<KitEntry>();
        List<KitEntry> timeoutKitEntriesToRemove = new List<KitEntry>();
        List<KitEntry> initialGlobalCooldownKitEntriesToRemove = new List<KitEntry>();

        if (Plugin.Instance.Config.ResetKitCooldownsOnRoundRestart)
        {
            foreach (var cooldownEntry in Plugin.Instance.KitManager.CooldownEntries)
            {
                if (cooldownEntry.Kit.ResetCooldownOnRoundRestart != false)
                {
                    cooldownEntriesToRemove.Add(cooldownEntry);
                }
            }
            foreach (var kitEntry in Plugin.Instance.KitManager.InitialCooldownKitEntries)
            {
                if (kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    initialCooldownKitEntriesToRemove.Add(kitEntry);
                }
            }
            foreach (var kitEntry in Plugin.Instance.KitManager.TimeoutKitEntries)
            {
                if (kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    timeoutKitEntriesToRemove.Add(kitEntry);
                }
            }

            foreach (var kitEntry in Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries)
            {
                if (kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    initialGlobalCooldownKitEntriesToRemove.Add(kitEntry);
                }
            }

            foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
            {
                if (kitEntry.InitialCooldown > 0f && kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    Plugin.Instance.KitManager.InitialCooldownKitEntries.Add(kitEntry);
                }

                if (kitEntry.GlobalKitTimeout > 0f && kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    Plugin.Instance.KitManager.TimeoutKitEntries.Add(kitEntry);
                }

                if (kitEntry.InitialGlobalCooldown > 0f && kitEntry.ResetCooldownOnRoundRestart != false)
                {
                    Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries.Add(kitEntry);
                }
            }
        }
        else
        {
            foreach (var cooldownEntry in Plugin.Instance.KitManager.CooldownEntries)
            {
                if (cooldownEntry.Kit.ResetCooldownOnRoundRestart == true)
                {
                    cooldownEntriesToRemove.Add(cooldownEntry);
                }
            }
            foreach (var kitEntry in Plugin.Instance.KitManager.InitialCooldownKitEntries)
            {
                if (kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    initialCooldownKitEntriesToRemove.Add(kitEntry);
                }
            }
            foreach (var kitEntry in Plugin.Instance.KitManager.TimeoutKitEntries)
            {
                if (kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    timeoutKitEntriesToRemove.Add(kitEntry);
                }
            }

            foreach (var kitEntry in Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries)
            {
                if (kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    initialGlobalCooldownKitEntriesToRemove.Add(kitEntry);
                }
            }

            foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
            {
                if (kitEntry.InitialCooldown > 0f && kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    Plugin.Instance.KitManager.InitialCooldownKitEntries.Add(kitEntry);
                }

                if (kitEntry.GlobalKitTimeout > 0f && kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    Plugin.Instance.KitManager.TimeoutKitEntries.Add(kitEntry);
                }

                if (kitEntry.InitialGlobalCooldown > 0f && kitEntry.ResetCooldownOnRoundRestart == true )
                {
                    Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries.Add(kitEntry);
                }
            }
        }
        
        Plugin.Instance.KitManager.CooldownEntries.RemoveAll(x => cooldownEntriesToRemove.Contains(x));
        Plugin.Instance.KitManager.InitialCooldownKitEntries.RemoveAll(x => initialCooldownKitEntriesToRemove.Contains(x));
        Plugin.Instance.KitManager.TimeoutKitEntries.RemoveAll(x => timeoutKitEntriesToRemove.Contains(x));
        Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries.RemoveAll(x => initialGlobalCooldownKitEntriesToRemove.Contains(x));
    }

    public void ResetKitUses()
    {
        List<KitUseEntry> kitUseEntriesToRemove = new List<KitUseEntry>();
        if (Plugin.Instance.Config.ResetKitUsesOnRoundRestart)
        {
            foreach (var kitUseEntry in Plugin.Instance.KitManager.KitUseEntries)
            {
                if (kitUseEntry.KitEntry.ResetUsesOnRoundRestart != false)
                {
                    kitUseEntriesToRemove.Add(kitUseEntry);
                }
            }
        }
        else
        {
            foreach (var kitUseEntry in Plugin.Instance.KitManager.KitUseEntries)
            {
                if (kitUseEntry.KitEntry.ResetUsesOnRoundRestart == true)
                {
                    kitUseEntriesToRemove.Add(kitUseEntry);
                }
            }
        }
        Plugin.Instance.KitManager.KitUseEntries.RemoveAll(x => kitUseEntriesToRemove.Contains(x));
    }

    public bool IsKitEntryOnCooldown(KitEntry kitEntry, Player player)
    {
        if (CooldownEntries == null)
        {
            return false;
        }

        CooldownEntry cooldownEntry = CooldownEntries.Find(x => x.Kit == kitEntry && x.Player == player);
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

        CooldownEntry cooldownEntry = CooldownEntries.Find(x => x.Player == player && x.Kit == kitEntry);
        return cooldownEntry.RemainingTime;
    }

    public CooldownEntry GetCooldownEntry(Player player, KitEntry kitEntry)
    {
        CooldownEntry cooldownEntry = CooldownEntries.Find(x => x.Player == player && x.Kit == kitEntry);
        return cooldownEntry;
    }

    public KitUseEntry GetKitUseEntry(Player player, KitEntry kitEntry)
    {
        KitUseEntry kitUseEntry = KitUseEntries.Find(x => x.Player == player && x.KitEntry == kitEntry);
        return kitUseEntry;
    }

    public void RemovePlayerEntryData(Player player)
    {
        foreach (var playerCooldownEntry in CooldownEntries.FindAll(x => x.Player == player))
        {
            CooldownEntries.Remove(playerCooldownEntry);
        }

        foreach (var playerKitUseEntry in KitUseEntries.FindAll(x => x.Player == player))
        {
            KitUseEntries.Remove(playerKitUseEntry);
        }

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
        RemainingTime = cooldownDuration;
        Timing.RunCoroutine(ReduceTime());
    }

    public CooldownEntry(Player player)
    {
        Player = player;
    }

    public CooldownEntry(Player player, KitEntry kitEntry)
    {
        Player = player;
        Kit = kitEntry;
    }

    public KitEntry Kit;
    public Player Player;
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
        Uses = uses;
    }

    public KitUseEntry(KitEntry kitEntry, Player player)
    {
        Player = player;
        KitEntry = kitEntry;
    }

    public KitEntry KitEntry;
    public Player Player;
    public int Uses;
}