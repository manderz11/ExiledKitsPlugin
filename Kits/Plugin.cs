using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;
using ExiledKitsPlugin.Handlers;

namespace ExiledKitsPlugin
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 1, 2);
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
        

        void RegisterEvents()
        {
            _handlers = new Handlers.Handlers();
            //Exiled.Events.Handlers.Server.RoundEnded += _handlers.OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += _handlers.OnRoundRestart;
            Exiled.Events.Handlers.Player.Left += _handlers.OnPlayerLeave;
            Exiled.Events.Handlers.Player.Spawned += _handlers.SpawnedEvent;
            Exiled.Events.Handlers.Server.RoundStarted += _handlers.RoundStarted;
        }

        void UnregisterEvents()
        {
            //Exiled.Events.Handlers.Server.RoundEnded -= _handlers.OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= _handlers.OnRoundRestart;
            Exiled.Events.Handlers.Player.Left -= _handlers.OnPlayerLeave;
            Exiled.Events.Handlers.Player.Spawned -= _handlers.SpawnedEvent;
            Exiled.Events.Handlers.Server.RoundStarted -= _handlers.RoundStarted;
            _handlers = null;
        }
    }
}