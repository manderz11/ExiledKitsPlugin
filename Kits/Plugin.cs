using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;
//using ExiledKitsPlugin.Handlers;

namespace ExiledKitsPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 0, 3);
        public static Plugin Instance { get; set; }
        public KitManager kitManager;
        //public KitCooldownManager kitCooldownManager;
        //private RoundRestartHandle RoundRestartHandle;
        
        public override void OnEnabled()
        {
            Instance = this;
            kitManager = new KitManager();
            kitManager.KitEntries = Config.Kits;
            //kitCooldownManager = new KitCooldownManager();
            //RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            kitManager = null;
            //kitCooldownManager = null;
            //UnregisterEvents();
            base.OnDisabled();
        }

        /*public override void OnReloaded()
        {
            
        }*/

        /*void RegisterEvents()
        {
            RoundRestartHandle = new RoundRestartHandle();
            Exiled.Events.Handlers.Server.RoundEnded += RoundRestartHandle.OnRoundEnded;
        }*/

        /*void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= RoundRestartHandle.OnRoundEnded;
            RoundRestartHandle = null;
        }*/
    }
}