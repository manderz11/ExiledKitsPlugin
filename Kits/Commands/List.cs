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
        if (!((CommandSender)sender).CheckPermission("kits.list"))
        {
            response = "You do not have permission (kits.list) to execute this command.";
            return false;
        }

        if (arguments.Count != 0)
        {
            response = "Entered too many arguments. Usage: kits list";
            return false;
        }

        if (Plugin.Instance.KitEntryManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        if (Plugin.Instance.KitEntryManager.KitEntries == null)
        {
            response = "Internal error. (Kit manager kit entries are null)";
            return false;
        }

        if (Plugin.Instance.KitEntryManager.KitEntries.Count > 0)
        {
            List<KitEntry> kitEntries = Plugin.Instance.KitEntryManager.KitEntries;
            string listResponse = "List of kits:\n";
            foreach (var entry in kitEntries)
            {
                listResponse += Plugin.Instance.KitEntryManager.FormattedKitContentList(entry);
            }
            
            response = listResponse;
            return true;
        }
        
        response = "No kits found in list.";
        return true;
    }
}