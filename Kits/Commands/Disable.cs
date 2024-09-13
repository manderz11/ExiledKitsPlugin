using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Commands;

public class Disable : ICommand
{
    public string Command { get; } = "disable";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Enables a kit";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("kits.disable"))
        {
            response = "You do not have permission (kits.disable) to execute this command.";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits disable (name)";
            return false;
        }

        if (Plugin.Instance.KitEntryManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        if (Plugin.Instance.KitEntryManager.GetKitEntryFromName(arguments.At(0)) == null)
        {
            response = "Could not find kit to disable with this name.";
            return false;
        }

        KitEntry kit = Plugin.Instance.KitEntryManager.GetKitEntryFromName(arguments.At(0));
        kit.Enabled = false;
        response = "Kit disabled!";
        return true;
    }
}