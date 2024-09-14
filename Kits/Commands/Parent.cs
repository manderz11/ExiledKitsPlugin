using System;
using CommandSystem;

namespace ExiledKitsPlugin.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(ClientCommandHandler))]
public class Parent : ParentCommand
{
    public Parent() => LoadGeneratedCommands();
    public override string Command { get; } = "kits";
    public override string[] Aliases { get; } = null;
    public override string Description { get; } = "Kit parent command/help command";
    private static Translation Translation => Plugin.Instance.Translation;

    public sealed override void LoadGeneratedCommands()
    {
        RegisterCommand(new List());
        RegisterCommand(new Give());
        RegisterCommand(new Delete());
        RegisterCommand(new Enable());
        RegisterCommand(new Disable());
        RegisterCommand(new Kit());
        RegisterCommand(new Debug());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        //response = "Use: kits (list | give | create | delete | edit | enable | disable | debug)";
        response = Translation.ParentCommand;
        return false;
    }
}