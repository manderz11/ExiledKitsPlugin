using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;
using ExiledKitsPlugin.Handlers;

namespace ExiledKitsPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 0, 4);
        public static Plugin Instance { get; set; }
        public KitManager KitManager;
        public KitCooldownManager KitCooldownManager;
        private RoundRestartHandle RoundRestartHandle;
        
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
            RoundRestartHandle = new RoundRestartHandle();
            Exiled.Events.Handlers.Server.RoundEnded += RoundRestartHandle.OnRoundEnded;
        }

        void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= RoundRestartHandle.OnRoundEnded;
            RoundRestartHandle = null;
        }
    }
}