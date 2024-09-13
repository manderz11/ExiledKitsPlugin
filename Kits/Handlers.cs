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
            Plugin.Instance.KitManager = new KitManager();
        }
    }

    public void SpawnedEvent(SpawnedEventArgs spawnedEventArgs)
    {
        Player p = spawnedEventArgs.Player;
        // ignore cooldown bypassed players
        if (p.CheckPermission("kits.give.cooldownbypass")) return;
        foreach (var kitEntry in Plugin.Instance.KitManager.InitialCooldownKitEntries)
        {
            Plugin.Instance.KitManager.StartKitCooldown(kitEntry,p,kitEntry.InitialCooldown);
            if (Plugin.Instance.Config.Debug) Log.Debug($"Starting kit cooldown for {kitEntry.Name} for {kitEntry.InitialCooldown} seconds");
        }
    }

    public void RoundStarted()
    {
        // clear round kit uses
        Plugin.Instance.KitManager.KitUses = new Dictionary<Dictionary<Player, KitEntry>, int>();
    }
}