using System;
using CommandSystem;

namespace ExiledKitsPlugin.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public class Parent : ParentCommand
{
    public Parent() => LoadGeneratedCommands();
    public override string Command { get; } = "kits";
    public override string[] Aliases { get; } = new[] { "kit" };
    public override string Description { get; } = "Kit parent command/help command";

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new List());
        RegisterCommand(new Give());
        RegisterCommand(new Delete());
        RegisterCommand(new Enable());
        RegisterCommand(new Disable());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        //response = "Use: kits (list | give | create | delete | edit | enable | disable)";
        response = "Use: kits (list | give | delete | enable | disable)";
        return false;
    }
}