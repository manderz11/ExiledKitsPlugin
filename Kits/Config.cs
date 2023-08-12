using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin
{
    public class Config : IConfig
    {
        [Description("Should plugin be enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Kit entries")]
        public List<KitEntry> Kits { get; set; } = new List<KitEntry>()
        {
            new KitEntry()
                { Name = "itemdemo", UsePermission = false, Ammo = null, Effects = null, Enabled = true, Items = new List<ItemType>() { ItemType.Coin, ItemType.Adrenaline } },
            new KitEntry()
                { Name = "ammodemo", UsePermission = false, Ammo = new Dictionary<AmmoType, ushort>() { { AmmoType.Nato9, 30}, { AmmoType.Nato556, 5}}, Effects = null, Enabled = true, Items = new List<ItemType>() { ItemType.GunCOM15, ItemType.Flashlight } },
            new KitEntry()
                { Name = "effectdemo", UsePermission = false, Ammo = new Dictionary<AmmoType, ushort>() { { AmmoType.Nato9, 60}, { AmmoType.Nato556, 15}}, Effects = new List<Effect>(){new Effect(EffectType.Scp207, 120f, 2), new Effect(EffectType.MovementBoost,120f,50)}, Enabled = true, Items = new List<ItemType>() { ItemType.GunFSP9, ItemType.Medkit } }
        };
        
        [Description("Should plugin show debug information?")]
        public bool Debug { get; set; } = false;
    }
}