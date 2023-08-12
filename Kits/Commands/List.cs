using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;

namespace ExiledKitsPlugin.Commands;

public class List : ICommand
{
    public string Command { get; } = "list";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Lists all aviable kits";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("kits.list"))
        {
            response = "You do not have permission (kits.list) to execute this command.";
            return false;
        }

        if (arguments.Count != 0)
        {
            response = "Entered too many arguments. Usage: kits list";
            return false;
        }

        if (Plugin.Instance.kitManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        if (Plugin.Instance.kitManager.KitEntries == null)
        {
            response = "Internal error. (Kit manager kit entries are null)";
            return false;
        }

        if (Plugin.Instance.kitManager.KitEntries.Count > 0)
        {
            List<KitEntry> kitEntries = Plugin.Instance.kitManager.KitEntries;
            string listResponse = "List of kits:\n";
            foreach (var entry in kitEntries)
            {
                listResponse += Plugin.Instance.kitManager.FormattedKitContentList(entry);
            }
            
            response = listResponse;
            return true;
        }
        
        response = "No kits found in list.";
        return true;
    }
}