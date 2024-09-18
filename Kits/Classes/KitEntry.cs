using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using JetBrains.Annotations;
using PlayerRoles;

namespace ExiledKitsPlugin.Classes
{
    public class KitEntry
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        [Description("Should the kit be permissions based")]
        public bool UsePermission { get; set; }
        [Description("Initial cooldown of the kit after player spawn, set to 0 to disable")]
        public float InitialCooldown { get; set; }
        [Description("Initial cooldown of the kit after game start, set to 0 to disable")]
        public int InitialGlobalCooldown { get; set; }
        [Description("Time after game start to timeout kit (disable redeeming), set to 0 to disable")]
        public int GlobalKitTimeout { get; set; }
        [Description("Time after player spawn to timeout kit (disable redeeming), set to 0 to disable")]
        public int SpawnKitTimeout { get; set; }
        [Description("Kit re-use cooldown in seconds, set to 0 to disable")]
        public float CooldownInSeconds { get; set; }
        [Description("Maximum number of uses per game, set to 0 to disable")]
        public int MaxUses { get; set; }
        [Description("Allowed roles to redeem kit")]
        [CanBeNull]public List<RoleTypeId> WhitelistedRoles { get; set; }
        [Description("Blacklisted roles to redeem kit, overrides whitelisted if the same role is set in whitelisted")]
        [CanBeNull]public List<RoleTypeId> BlacklistedRoles { get; set; }
        [Description("Should kit delete all existing inventory items")]
        public bool ClearInventory { get; set; }
        [Description("Should excess items be dropped")]
        public bool DropExcess { get; set; }
        [CanBeNull] public List<ItemType> Items { get; set; }
        [CanBeNull] public Dictionary<AmmoType, ushort> Ammo { get; set; }
        [CanBeNull] public List<Effect> Effects { get; set; }
        [CanBeNull] public RoleTypeId? SetRole { get; set; }
    }
}