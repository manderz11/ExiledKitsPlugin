using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace ExiledKitsPlugin.Classes;

public class KitEntryManager
{
    public List<KitEntry> KitEntries;

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
        if (kit.SetRole != RoleTypeId.None && kit.SetRole != null)
        {
            player.Role.Set((RoleTypeId)kit.SetRole,SpawnReason.ForceClass,RoleSpawnFlags.None);
        }
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
            //player.EnableEffects(kit.Effects); obsolete as of exiled 8.3.x
            player.SyncEffects(kit.Effects);
        }
    }

    public string FormattedKitContentList(KitEntry kit)
    {
        string formatted = $"<color=#32CD32># {kit.Name} (Enabled: {kit.Enabled}) contents:</color>\n";
        formatted += $"<color=#FFFFFF># Settings: Enabled: {kit.Enabled} Override inventory: {kit.OverrideInventory} Drop overriden items: {kit.DropOverridenItems}</color>\n" +
                     $"<color=#FFFFFF># Use permission: {kit.UsePermission} Cooldown: {kit.CooldownInSeconds} Initial cooldown: {kit.InitialCooldown} Initial global cooldown: {kit.InitialGlobalCooldown}</color>\n" +
                     $"<color=#FFFFFF># Kit spawn timeout: {kit.SpawnKitTimeout} Kit global timeout: {kit.GlobalKitTimeout}</color>\n";
        if (kit.WhitelistedRoles != null)
        {
            formatted += "<color=#808080># Whitelisted roles:</color>\n";
            foreach (var role in kit.WhitelistedRoles)
            {
                formatted += $"-{role.ToString()}\n";
            }
        }

        if (kit.BlacklistedRoles != null)
        {
            formatted += "<color=#808080># Blacklisted roles:</color>\n";
            foreach (var role in kit.BlacklistedRoles)
            {
                formatted += $"-{role.ToString()}\n";
            }
        }
        
        if (kit.Items != null)
        {
            formatted += "<color=#DC143C># Items:</color>\n";
            foreach (var item in kit.Items)
            {
                formatted += $"-{item.ToString()}\n";
            }
        }

        if (kit.Ammo != null)
        {
            formatted += "<color=#DC143C># Ammo:</color>\n";
            foreach (var ammo in kit.Ammo)
            {
                formatted += $"-({ammo.Value}x) {ammo.Key.ToString()}\n";
            }
        }

        if (kit.Effects != null)
        {
            formatted += "<color=#DC143C># Effects:</color>\n";
            foreach (var effect in kit.Effects)
            {
                formatted += $"-({effect.Intensity}x) {effect.Type.ToString()} {effect.Duration}s\n";
            }
        }

        if (kit.SetRole != RoleTypeId.None && kit.SetRole != null)
        {
            formatted += $"<color=#DC143C># Sets role to: {kit.SetRole}</color>\n";
        }
        return formatted;
    }

    public KitEntry GetKitEntryFromName(string name)
    {
        KitEntry kitEntry;
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