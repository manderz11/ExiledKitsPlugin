﻿using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 1, 8);
        public static Plugin Instance { get; set; }
        public KitEntryManager KitEntryManager;
        public KitManager KitManager;
        private Handlers _handlers;
        
        public override void OnEnabled()
        {
            Instance = this;
            KitEntryManager = new KitEntryManager();
            KitEntryManager.KitEntries = Config.Kits;
            KitManager = new KitManager();
            RegisterEvents();
            // fix spawn time if plugin is reloaded
            foreach (var player in Player.List)
            {
                if (!player.IsAlive) continue;
                KitManager.PlayerSpawnTime.Add(player,Round.ElapsedTime.TotalSeconds);
            }
            if(Config.Debug) Log.Debug("Plugin enabled");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            KitEntryManager = null;
            KitManager = null;
            UnregisterEvents();
            if(Config.Debug) Log.Debug("Plugin disabled");
            base.OnDisabled();
        }

        void RegisterEvents()
        {
            _handlers = new Handlers();
            Exiled.Events.Handlers.Server.RestartingRound += _handlers.OnRoundRestart;
            Exiled.Events.Handlers.Player.Left += _handlers.OnPlayerLeave;
            Exiled.Events.Handlers.Player.Spawned += _handlers.SpawnedEvent;
        }

        void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RestartingRound -= _handlers.OnRoundRestart;
            Exiled.Events.Handlers.Player.Left -= _handlers.OnPlayerLeave;
            Exiled.Events.Handlers.Player.Spawned -= _handlers.SpawnedEvent;
            _handlers = null;
        }
    }
}