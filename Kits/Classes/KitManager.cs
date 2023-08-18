using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace ExiledKitsPlugin.Classes;

public class KitManager
{
    public List<KitEntry> KitEntries;

    public List<KitEntry> GetKitEntries()
    {
        return KitEntries;
    }

    public bool DeleteKit(KitEntry kitEntry)
    {
        if (KitEntries.Contains(kitEntry))
        {
            KitEntries.Remove(kitEntry);
            return true;
        }
        return false;
    }
    
    public void GiveKitContents(Player player, KitEntry kit)
    {
        if (kit.Items != null)
        {
            player.AddItem(kit.Items);
        }
        if (kit.Ammo != null)
        {
            foreach (var entry in kit.Ammo)
            {
                player.AddAmmo(entry.Key, entry.Value);
            }
        }
        if (kit.Effects != null)
        {
            player.EnableEffects(kit.Effects);
        }
    }

    public string FormattedKitContentList(KitEntry kit)
    {
        string formatted = $"<size=125%><color=#32CD32># {kit.Name} (Enabled: {kit.Enabled}) contents:</color></size>\n";
        if (kit.Items != null)
        {
            formatted += "<size=110%><color=#DC143C># Items:</color></size>\n";
            foreach (var item in kit.Items)
            {
                formatted += $"-{item.ToString()}\n";
            }
        }

        if (kit.Ammo != null)
        {
            formatted += "<size=110%><color=#DC143C># Ammo:</color></size>\n";
            foreach (var ammo in kit.Ammo)
            {
                formatted += $"-({ammo.Value}x) {ammo.Key.ToString()}\n";
            }
        }

        if (kit.Effects != null)
        {
            formatted += "<size=110%><color=#DC143C># Effects:</color></size>\n";
            foreach (var effect in kit.Effects)
            {
                formatted += $"-({effect.Intensity}x) {effect.Type.ToString()} {effect.Duration}s\n";
            }
        }
        return formatted;
    }

    public KitEntry GetKitEntryFromName(string name)
    {
        KitEntry kitEntry = null;
        try
        {
            kitEntry = KitEntries.First(x => x.Name == name);
        }
        catch (Exception e)
        {
            if (Plugin.Instance.Config.Debug)
            {
                Log.Debug($"Kit entry was not found, exception: {e}");
            }
            return null;
        }

        return kitEntry;
    }
}