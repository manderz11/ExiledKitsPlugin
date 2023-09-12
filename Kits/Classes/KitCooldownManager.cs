/*using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace ExiledKitsPlugin.Classes;

public class KitCooldownManager
{
    private Dictionary<Player, List<CooldownEntry>> _kitEntryCooldowns = new Dictionary<Player, List<CooldownEntry>>();

    public bool IsKitEntryOnCooldown(KitEntry kitEntry, Player player)
    {
        if (kitEntry.CooldownInSeconds <= -1)
        {
            return false;
        }

        if (!_kitEntryCooldowns.ContainsKey(player))
        {
            return false;
        }

        if (_kitEntryCooldowns.TryGetValue(player, out List<CooldownEntry> cooldownEntries))
        {
            foreach (var entry in cooldownEntries)
            {
                if (entry.KitEntry == kitEntry)
                {
                    if (entry.TimeLeft > 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        else
        {
            return false;
        }
    }

    public void StartKitCooldown(KitEntry kitEntry, Player player, float time)
    {
        if (_kitEntryCooldowns.TryGetValue(player, out List<CooldownEntry> cooldownEntries))
        {
            foreach (var entry in cooldownEntries)
            {
                if (entry.KitEntry == kitEntry)
                {
                    
                    cooldownEntries.Remove(entry);
                    CooldownEntry cooldownEntry = new CooldownEntry() { KitEntry = kitEntry };
                    Timing.RunCoroutine(cooldownEntry.CooldownCountdown(time));
                    cooldownEntries.Add(cooldownEntry);
                    _kitEntryCooldowns.Remove(player);
                    _kitEntryCooldowns.Add(player,cooldownEntries);
                    return;
                    
                }
            }
        }
        else
        {
            CooldownEntry cooldownEntry = new CooldownEntry() {KitEntry = kitEntry};
            Timing.RunCoroutine(cooldownEntry.CooldownCountdown(time));
            _kitEntryCooldowns.Add(player,new List<CooldownEntry>(){cooldownEntry});
        }
    }

    public float GetTimeLeft(KitEntry kitEntry, Player player)
    {
        if (kitEntry.CooldownInSeconds <= -1f)
        {
            return -1f;
        }

        if (!_kitEntryCooldowns.ContainsKey(player))
        {
            return -1f;
        }

        if (_kitEntryCooldowns.TryGetValue(player, out List<CooldownEntry> cooldownEntries))
        {
            foreach (var entry in cooldownEntries)
            {
                if (entry.KitEntry == kitEntry)
                {
                    if (Plugin.Instance.Config.Debug)
                    {
                        Log.Debug($"timeleft {entry.TimeLeft} on kit {entry.KitEntry.Name}");
                    }
                    return entry.TimeLeft;
                }
            }

            return -1f;
        }
        else
        {
            return -1f;
        }
    }
}

public class CooldownEntry
{
    public KitEntry KitEntry;
    public float TimeLeft;

    public IEnumerator<float> CooldownCountdown(float time)
    {
        TimeLeft = time;
        while (TimeLeft > 0f)
        {
            TimeLeft -= 1f;
            if (Plugin.Instance.Config.Debug)
            {
                Log.Debug($"{time} is timeleft on kit {KitEntry.Name}");
            }
            yield return Timing.WaitForSeconds(1);
        }
    }
}*/