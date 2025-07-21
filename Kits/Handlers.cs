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
        Plugin.Instance.KitManager = new KitManager();
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