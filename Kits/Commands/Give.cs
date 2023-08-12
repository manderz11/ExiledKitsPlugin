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
        if (!sender.CheckPermission("kits.give"))
        {
            response = "You do not have permission (kits.give) to execute this command.";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits give (UserId | in-game id) (kit name)";
            return false;
        }

        if (Plugin.Instance.kitManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        int.TryParse(arguments.At(0), out var usernetid);
        ReferenceHub refPlayer = ReferenceHub.GetHub(usernetid);
        if (refPlayer == null)
        {
            response = "incorrect userid";
            return false;
        }

        var arg = arguments.At(1);
        if (Plugin.Instance.kitManager.GetKitEntryFromName(arg) == null)
        {
            response = "Kit with specified name could not be found";
            return false;
        }
        KitEntry kit = Plugin.Instance.kitManager.GetKitEntryFromName(arg);
        if (kit != null)
        {
            if (!kit.Enabled && !sender.CheckPermission("kits.givebypass"))
            {
                response = "This kit is not enabled and cannot be redeemed.";
                return false;
            }

            if (kit.UsePermission)
            {
                // When kit use permission is enabled, check if sender can give this kit
                // Example, test kit would require kits.give.test
                if (!sender.CheckPermission($"kits.give.{kit.Name}"))
                {
                    response = $"You do not have permission (kits.give.{kit.Name}) to give this kit";
                    return false;
                }
            }
            Player player = Player.Get(refPlayer);
            Plugin.Instance.kitManager.GiveKitContents(player, kit);
            response = $"Gave kit {kit.Name} to {player.Nickname}";
            return true;
        }
        
        response = "Unknown internal error.";
        return false;
    }
}