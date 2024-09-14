using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;
using MEC;

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
        if (Plugin.Instance.KitManager.PlayerSpawnTime.ContainsKey(p))
        {
            Plugin.Instance.KitManager.PlayerSpawnTime.Remove(p);
            Plugin.Instance.KitManager.PlayerSpawnTime.Add(p,Round.ElapsedTime.TotalSeconds);
        }
        else
        {
            Plugin.Instance.KitManager.PlayerSpawnTime.Add(p,Round.ElapsedTime.TotalSeconds);
        }
        if(spawnedEventArgs.Reason == SpawnReason.ForceClass){return;}
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
}