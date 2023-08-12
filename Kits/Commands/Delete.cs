using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Commands;

public class Delete : ICommand
{
    public string Command { get; } = "delete";
    public string[] Aliases { get; } = new []{"remove"};
    public string Description { get; } = "Removes a kit";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("kits.delete"))
        {
            response = "You do not have permission (kits.delete) to execute this command.";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits delete (name)";
            return false;
        }

        if (Plugin.Instance.kitManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        if (Plugin.Instance.kitManager.GetKitEntryFromName(arguments.At(0)) == null)
        {
            response = "Could not find kit to delete with this name.";
            return false;
        }

        KitEntry kit = Plugin.Instance.kitManager.GetKitEntryFromName(arguments.At(0));

        if (Plugin.Instance.kitManager.DeleteKit(kit))
        {
            response = "Kit successfully deleted";
            return true;
        }
        response = "Kit failed to be deleted. (Possibly entry not found in list)";
        return false;
    }
}