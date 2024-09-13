﻿using System;
using CommandSystem;

namespace ExiledKitsPlugin.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(ClientCommandHandler))]
public class Parent : ParentCommand
{
    public Parent() => LoadGeneratedCommands();
    public override string Command { get; } = "kits";
    public override string[] Aliases { get; } = null; //new[] { "kit" }; -- replaced for kit
    public override string Description { get; } = "Kit parent command/help command";

    public sealed override void LoadGeneratedCommands()
    {
        RegisterCommand(new List());
        RegisterCommand(new Give());
        RegisterCommand(new Delete());
        RegisterCommand(new Enable());
        RegisterCommand(new Disable());
        RegisterCommand(new Kit());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        //response = "Use: kits (list | give | create | delete | edit | enable | disable)";
        response = "Use: kits (list | give | delete | enable | disable)";
        return false;
    }
}