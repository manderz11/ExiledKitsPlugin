using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Handlers;

public class Handlers
{
    public void OnRoundEnded(RoundEndedEventArgs roundEndedEventArgs)
    {
        if (Plugin.Instance.Config.ResetCooldownsOnRoundEnd)
        {
            Plugin.Instance.KitCooldownManager = new KitCooldownManager();
        }
    }

    public void SpawnedEvent(SpawnedEventArgs spawnedEventArgs)
    {
        Player p = spawnedEventArgs.Player;
        // ignore cooldown bypassed players
        if (p.CheckPermission("kits.give.cooldownbypass")) return;
        foreach (var kitEntry in Plugin.Instance.KitCooldownManager.InitialCooldownKitEntries)
        {
            Plugin.Instance.KitCooldownManager.StartKitCooldown(kitEntry,p,kitEntry.InitialCooldown);
            if (Plugin.Instance.Config.Debug) Log.Debug($"Starting kit cooldown for {kitEntry.Name} for {kitEntry.InitialCooldown} seconds");
        }
    }
}