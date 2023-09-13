using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Commands;

public class Enable : ICommand
{
    public string Command { get; } = "enable";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Enables a kit";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("kits.enable"))
        {
            response = "You do not have permission (kits.enable) to execute this command.";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits enable (name)";
            return false;
        }

        if (Plugin.Instance.KitManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        if (Plugin.Instance.KitManager.GetKitEntryFromName(arguments.At(0)) == null)
        {
            response = "Could not find kit to enable with this name.";
            return false;
        }

        KitEntry kit = Plugin.Instance.KitManager.GetKitEntryFromName(arguments.At(0));
        kit.Enabled = true;
        response = "Kit enabled!";
        return true;
    }
}