using System;
using Exiled.API.Features;
using ExiledKitsPlugin.Classes;
using ExiledKitsPlugin.Handlers;

namespace ExiledKitsPlugin
{
    /* TODO for 1.1.0, as of 1.0.6/1.0.7-pre
    1. initial kit cooldowns -- done
    2. giving without using kit parent command -- done
    3. probably some refactoring of a year old code -- in progress
    4. global kit cooldown -- done
    5. kit timeout cooldown -- done
    6. max kit uses -- done
    7. blacklisted ktis though it would conflict with whitelisted in a way but is more covenient -- done
    8. role setting -- done
    9. change permissions to more appropriate permissions -- done
    10. replace kitmanagers gamerunning with Round.ElapsedTime -- done
    11. add timeout after spawn -- done
    12. implement translations
     */
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Kits";
        public override string Author => "manderz11";
        public override Version Version => new Version(1, 1, 0);
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