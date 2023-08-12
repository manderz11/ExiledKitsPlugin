using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 0, 0);
        public static Plugin Instance { get; set; }
        internal KitManager kitManager;
        
        public override void OnEnabled()
        {
            Instance = this;
            kitManager = new KitManager();
            kitManager.KitEntries = Config.Kits;
            
        }

        public override void OnDisabled()
        {
            Instance = null;
            kitManager = null;
        }

        /*public override void OnReloaded()
        {
            
        }*/
    }
}