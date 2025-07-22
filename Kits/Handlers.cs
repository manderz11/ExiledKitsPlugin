using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin;

public class Handlers
{
    public void OnRoundRestart()
    {
        if(Plugin.Instance.Config.ResetKitUsesOnRoundRestart)Plugin.Instance.KitManager.KitUseEntries = new List<KitUseEntry>();
        if (Plugin.Instance.Config.ResetKitCooldownsOnRoundRestart)
        {
            Plugin.Instance.KitManager.CooldownEntries = new List<CooldownEntry>();
            Plugin.Instance.KitManager.InitialCooldownKitEntries = new List<KitEntry>();
            Plugin.Instance.KitManager.TimeoutKitEntries = new List<KitEntry>();
            Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries = new List<KitEntry>();
            foreach (var kitEntry in Plugin.Instance.KitEntryManager.KitEntries)
            {
                if (kitEntry.InitialCooldown > 0f)
                {
                    Plugin.Instance.KitManager.InitialCooldownKitEntries.Add(kitEntry);
                }

                if (kitEntry.GlobalKitTimeout > 0f)
                {
                    Plugin.Instance.KitManager.TimeoutKitEntries.Add(kitEntry);
                }

                if (kitEntry.InitialGlobalCooldown > 0f)
                {
                    Plugin.Instance.KitManager.InitialGlobalCooldownKitEntries.Add(kitEntry);
                }
            }
        }
    }

    public void SpawnedEvent(SpawnedEventArgs spawnedEventArgs)
    {
        Player p = spawnedEventArgs.Player;
        // ignore cooldown bypassed players
        if (p.CheckPermission("kits.give.cooldownbypass")) return;
        if (spawnedEventArgs.Reason == SpawnReason.Died)
        {
            if (Plugin.Instance.Config.Debug) Log.Debug($"Player {spawnedEventArgs.Player.Nickname} died! Skipping spawnedevent. Reason: {spawnedEventArgs.Reason}. Possibly player left.");
            return;
        }
        if (Plugin.Instance.KitManager.PlayerSpawnTime.ContainsKey(p))
        {
            Plugin.Instance.KitManager.PlayerSpawnTime.Remove(p);
            Plugin.Instance.KitManager.PlayerSpawnTime.Add(p,Round.ElapsedTime.TotalSeconds);
        }
        else
        {
            Plugin.Instance.KitManager.PlayerSpawnTime.Add(p,Round.ElapsedTime.TotalSeconds);
        }
        foreach (var kitEntry in Plugin.Instance.KitManager.InitialCooldownKitEntries)
        {
            Plugin.Instance.KitManager.StartKitCooldown(kitEntry,p,kitEntry.InitialCooldown);
            if (Plugin.Instance.Config.Debug) Log.Debug($"Starting kit cooldown for {kitEntry.Name} for {kitEntry.InitialCooldown} seconds");
        }
    }

    public void RoundStarted()
    {
        // clear round kit uses
        Plugin.Instance.KitManager.KitUseEntries = new List<KitUseEntry>();
    }

    public void OnPlayerLeave(LeftEventArgs leftEventArgs)
    {
        if(Plugin.Instance.Config.Debug)Log.Debug($"Player {leftEventArgs.Player.Nickname} left! Clearing all data.");
        Plugin.Instance.KitManager.RemovePlayerEntryData(leftEventArgs.Player);
    }
}