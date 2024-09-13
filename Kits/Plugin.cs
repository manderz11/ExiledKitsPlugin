﻿using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;
using ExiledKitsPlugin.Handlers;

namespace ExiledKitsPlugin
{
    /* TODO for 1.1.0, as of 1.0.6/1.0.7-pre
    1. initial kit cooldowns -- done
    2. giving without using kit parent command -- done
    3. probably some refactoring of a year old code -- in progress
    4. global kit cooldown
    5. kit timeout cooldown
    6. max kit uses -- done
     */
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 0, 7);
        public static Plugin Instance { get; set; }
        public KitEntryManager KitEntryManager;
        public KitManager KitManager;
        private Handlers.Handlers _handlers;
        
        public override void OnEnabled()
        {
            Instance = this;
            KitEntryManager = new KitEntryManager();
            KitEntryManager.KitEntries = Config.Kits;
            KitManager = new KitManager();
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            KitEntryManager = null;
            KitManager = null;
            UnregisterEvents();
            base.OnDisabled();
        }

        /*public override void OnReloaded()
        {
            
        }*/

        void RegisterEvents()
        {
            _handlers = new Handlers.Handlers();
            Exiled.Events.Handlers.Server.RoundEnded += _handlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Spawned += _handlers.SpawnedEvent;
            Exiled.Events.Handlers.Server.RoundStarted += _handlers.RoundStarted;
        }

        void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= _handlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Spawned -= _handlers.SpawnedEvent;
            Exiled.Events.Handlers.Server.RoundStarted -= _handlers.RoundStarted;
            _handlers = null;
        }
    }
}