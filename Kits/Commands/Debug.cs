using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ExiledKitsPlugin.Commands;

public class Debug : ICommand
{
    public string Command { get; } = "debug";
    public string[] Aliases { get; } = null;
    public string Description { get; } = "Shows debug information about the plugin";
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (!((CommandSender)sender).CheckPermission("kits.debug"))
        {
            response = "You do not have permission to run command debug.";
            return false;
        }

        string formatted = "Debug information:\n";
        formatted += $"Round running time: {Round.ElapsedTime.Seconds}s\n";
        formatted += "Kit uses:\n";
        foreach (var kitUses in Plugin.Instance.KitManager.KitUseEntries)
        {
            formatted += $"Player: {kitUses.Player.Nickname}, {kitUses.KitEntry.Name} kit, uses {kitUses.Uses} times\n\n";
        }

        formatted += "Cooldown entries:\n";
        foreach (var cooldownEntry in Plugin.Instance.KitManager.CooldownEntries)
        {
            formatted += $"Player: {cooldownEntry.Player.Nickname}, {cooldownEntry.Kit.Name} kit, {cooldownEntry.RemainingTime} time\n\n";
        }
        response = formatted;
        return true;
    }
}