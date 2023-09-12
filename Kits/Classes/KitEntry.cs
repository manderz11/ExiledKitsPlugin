using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using JetBrains.Annotations;

namespace ExiledKitsPlugin.Classes
{
    public class KitEntry
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public bool UsePermission { get; set; }
        public float CooldownInSeconds { get; set; }
        [CanBeNull] public List<ItemType> Items { get; set; }
        [CanBeNull] public Dictionary<AmmoType, ushort> Ammo { get; set; }
        [CanBeNull] public List<Effect> Effects { get; set; }
    }
}