using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Interfaces;
using ExiledKitsPlugin.Classes;
using PlayerRoles;

namespace ExiledKitsPlugin
{
    public class Config : IConfig
    {
        [Description("Should plugin be enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Should cooldowns be reset after the round has ended")]
        public bool ResetCooldownsOnRoundEnd { get; set; } = false;
        
        [Description("Kit entries")]
        public List<KitEntry> Kits { get; set; } = new List<KitEntry>()
        {
            new KitEntry()
                { Name = "itemdemo", UsePermission = false, Ammo = null, Effects = null, Enabled = true, OverrideInventory = false, WhitelistedRoles = null, Items = new List<ItemType>() { ItemType.Coin, ItemType.Adrenaline }, CooldownInSeconds = 1800f},
            new KitEntry()
                { Name = "ammodemo", UsePermission = true, Ammo = new Dictionary<AmmoType, ushort>() { { AmmoType.Nato9, 30}, { AmmoType.Nato556, 5}}, Effects = null, Enabled = true, OverrideInventory = true, DropOverridenItems = false, WhitelistedRoles = null,Items = new List<ItemType>() { ItemType.GunCOM15, ItemType.Flashlight }, CooldownInSeconds = 60f},
            new KitEntry()
                { Name = "effectdemo", UsePermission = false, Ammo = new Dictionary<AmmoType, ushort>() { { AmmoType.Nato9, 60}, { AmmoType.Nato556, 15}}, Effects = new List<Effect>(){new Effect(EffectType.Scp207, 120f, 2), new Effect(EffectType.MovementBoost,120f,50)}, Enabled = true, OverrideInventory = true, DropOverridenItems = true, WhitelistedRoles = null, Items = new List<ItemType>() { ItemType.GunFSP9, ItemType.Medkit }, CooldownInSeconds = -1f},
            new KitEntry()
                {Name = "ChaoticRevolt", UsePermission = true, OverrideInventory = true, DropOverridenItems = true, CooldownInSeconds = 30, Enabled = true, Items = new List<ItemType>(){ItemType.Adrenaline, ItemType.KeycardMTFOperative, ItemType.GunA7, ItemType.ArmorHeavy, ItemType.Medkit, ItemType.Medkit}, Ammo = new Dictionary<AmmoType, ushort>(){{AmmoType.Nato762, 120},
                    { AmmoType.Nato556, 60}}, Effects = new List<Effect>(){new Effect(EffectType.Scp207, 0f, 3, false, true)}, WhitelistedRoles = new List<RoleTypeId>() {RoleTypeId.ClassD, RoleTypeId.ChaosConscript, RoleTypeId.ChaosMarauder, RoleTypeId.ChaosRepressor, RoleTypeId.ChaosRifleman}}
        };
        
        [Description("Should plugin show debug information?")]
        public bool Debug { get; set; } = false;
    }
}