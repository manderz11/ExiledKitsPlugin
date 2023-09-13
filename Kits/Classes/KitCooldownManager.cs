using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace ExiledKitsPlugin.Classes;

public class KitCooldownManager
{
    public List<CooldownEntry> CooldownEntries = new List<CooldownEntry>();
    
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
        while (RemainingTime > 0)
        {
            RemainingTime -= 1;
            yield return Timing.WaitForSeconds(1);
        }
    }
}