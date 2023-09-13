using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Commands;

public class Give : ICommand
{
    public string Command { get; } = "give";
    public string[] Aliases { get; } = new []{"givekit"};
    public string Description { get; } = "Gives a kit to a player";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits give (UserId | in-game id) (kit name)";
            return false;
        }

        if (Plugin.Instance.KitManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        var kitNameArg = arguments.At(1);
        if (Plugin.Instance.KitManager.GetKitEntryFromName(kitNameArg) == null)
        {
            response = "Kit with specified name could not be found";
            return false;
        }
        
        KitEntry kit = Plugin.Instance.KitManager.GetKitEntryFromName(kitNameArg);
        if (kit != null)
        {
            // When kit use permission is enabled, check if sender can give this kit
            if (kit.UsePermission)
            {
                if (!((CommandSender)sender).CheckPermission($"kits.give.{kit.Name}"))
                {
                    response = $"You do not have permission to redeem this kit (kits.give.{kit.Name})";
                    return false;
                }
            }
            else
            {
                if (!((CommandSender)sender).CheckPermission("kits.give"))
                {
                    response = "You do not have permission (kits.give) to execute this command.";
                    return false;
                }
            }
        }
        else
        {
            response = "<color=red>ERROR: Kit entry is null even though that should be impossible?!?!</color>";
            return false;
        }
        
        ReferenceHub refPlayer = null;
        Player player;
        if (int.TryParse(arguments.At(0), out var usernetid))
        {
            refPlayer = ReferenceHub.GetHub(usernetid);
        }
        else
        {
            if (Player.TryGet(arguments.At(0), out player))
            {
                refPlayer = ReferenceHub.GetHub(player.GameObject);
            }
        }
        
        if (refPlayer == null)
        {
            response = "Player could not be found by name or id.";
            return false;
        }
        
        if (!kit.Enabled && !((CommandSender)sender).CheckPermission("kits.givebypass"))
        {
            response = "This kit is not enabled and cannot be redeemed.";
            return false;
        }
            
        if (kit.UsePermission)
        {
            
            if (!((CommandSender)sender).CheckPermission($"kits.give.{kit.Name}"))
            {
                response = $"You do not have permission (kits.give.{kit.Name}) to give this kit";
                return false;
            }
        }

        player = Player.Get(refPlayer);
        
        if (kit.CooldownInSeconds > -1f)
        {
            if (Plugin.Instance.KitCooldownManager.IsKitEntryOnCooldown(kit,player))
            {
                if (!((CommandSender)sender).CheckPermission("kits.give.cooldownbypass"))
                {
                    CooldownEntry cooldownEntry = Plugin.Instance.KitCooldownManager.GetCooldownEntry(player, kit);
                    response = $"This kit is on cooldown for {cooldownEntry.RemainingTime}s";
                    return false;
                }
            }
        }
        
        Plugin.Instance.KitManager.GiveKitContents(player, kit);
        response = $"Gave kit {kit.Name} to {player.Nickname}";

        if (!((CommandSender)sender).CheckPermission("kits.give.cooldownbypass"))
        {
            if (kit.CooldownInSeconds > -1f)
            {
                Plugin.Instance.KitCooldownManager.StartKitCooldown(kit, player, kit.CooldownInSeconds);
            }
        }
        
        
        return true;
    }
}