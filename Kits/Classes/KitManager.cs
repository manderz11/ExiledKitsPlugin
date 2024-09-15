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
        // reset all cooldowns between games
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