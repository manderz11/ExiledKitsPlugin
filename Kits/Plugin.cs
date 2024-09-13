using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;
using ExiledKitsPlugin.Handlers;

namespace ExiledKitsPlugin
{
    /* TODO as of 1.0.7
    1. initial kit cooldowns
    2. giving without using kit parent command
    3. probably some refactoring of a year old code
     */
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 0, 7);
        public static Plugin Instance { get; set; }
        public KitManager KitManager;
        public KitCooldownManager KitCooldownManager;
        private Handlers.Handlers _handlers;
        
        public override void OnEnabled()
        {
            Instance = this;
            KitManager = new KitManager();
            KitManager.KitEntries = Config.Kits;
            KitCooldownManager = new KitCooldownManager();
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            KitManager = null;
            KitCooldownManager = null;
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
        }

        void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= _handlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Spawned -= _handlers.SpawnedEvent;
            _handlers = null;
        }
    }
}