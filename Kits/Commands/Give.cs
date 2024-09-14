using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using ExiledKitsPlugin.Classes;
using PlayerRoles;

namespace ExiledKitsPlugin.Commands;

public class Give : ICommand
{
    public string Command { get; } = "give";
    public string[] Aliases { get; } = new []{"givekit"};
    public string Description { get; } = "Gives a kit to a player";
    private static Translation Translation => Plugin.Instance.Translation;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits give (UserId | in-game id) (kit name)";
            return false;
        }

        if (Plugin.Instance.KitEntryManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        var kitNameArg = arguments.At(1);
        if (Plugin.Instance.KitEntryManager.GetKitEntryFromName(kitNameArg) == null)
        {
            //response = "Kit with specified name could not be found";
            response = Translation.InvalidKitName;
            return false;
        }
        
        KitEntry kit = Plugin.Instance.KitEntryManager.GetKitEntryFromName(kitNameArg);
        if (kit != null)
        {
            if (kit.UsePermission)
            {
                if (!((CommandSender)sender).CheckPermission($"kits.give.{kit.Name}"))
                {
                    //response = $"You do not have permission to redeem this kit (kits.give.{kit.Name})";
                    response = Translation.InvalidKitPermission.Replace("%permission%", $"kits.give.{kit.Name}");
                    return false;
                }
            }
            else
            {
                if (!((CommandSender)sender).CheckPermission("kits.give"))
                {
                    //response = "You do not have permission (kits.give) to execute this command.";
                    response = Translation.InvalidGivePermissions;
                    return false;
                }
            }
        }
        else
        {
            response = "<color=red>ERROR: Kit entry is null even though that should be impossible?!?!</color>";
            return false;
        }
        
        ReferenceHub refPlayer = null;
        Player player;
        if (int.TryParse(arguments.At(0), out var usernetid))
        {
            refPlayer = ReferenceHub.GetHub(usernetid);
        }
        else
        {
            if (Player.TryGet(arguments.At(0), out player))
            {
                refPlayer = ReferenceHub.GetHub(player.GameObject);
            }
        }
        
        if (refPlayer == null)
        {
            response = "Player could not be found by name or id.";
            return false;
        }
        
        if (!kit.Enabled && !((CommandSender)sender).CheckPermission("kits.give.givebypass"))
        {
            //response = "This kit is not enabled and cannot be redeemed.";
            response = Translation.KitNotEnabled;
            return false;
        }

        player = Player.Get(refPlayer);
        
        if (!Round.IsStarted)
        {
            if (kit.WhitelistedRoles == null)
            {
                if (!player.CheckPermission("kits.give.givebypass"))
                {
                    //response = "Kits cannot be redeemed before the round has started.";
                    response = Translation.KitRoundStart;
                    return false;
                }
            }
            else
            {
                if (kit.WhitelistedRoles.Contains(RoleTypeId.Scientist))
                {
                    if (!player.CheckPermission("kits.give.givebypass"))
                    {
                        //response = "Kits cannot be redeemed before the round has started.";
                        response = Translation.KitRoundStart;
                        return false;
                    }
                }
            }
        }
        
        if (kit.CooldownInSeconds > -1f || kit.InitialCooldown > -1f)
        {
            if (Plugin.Instance.KitManager.IsKitEntryOnCooldown(kit,player))
            {
                if (!((CommandSender)sender).CheckPermission("kits.give.cooldownbypass"))
                {
                    CooldownEntry cooldownEntry = Plugin.Instance.KitManager.GetCooldownEntry(player, kit);
                    //response = $"This kit is on cooldown for {cooldownEntry.RemainingTime}s";
                    response = Translation.KitCooldown.Replace("%cooldown%",cooldownEntry.RemainingTime.ToString());
                    return false;
                }
            }
        }
        
        if (kit.GlobalKitTimeout > 0)
        {
            if (kit.GlobalKitTimeout < Round.ElapsedTime.TotalSeconds)
            {
                if (!player.CheckPermission("kits.give.timeoutbypass"))
                {
                    //response = $"You cannot use this kit after {kit.GlobalKitTimeout}s of the game starting. The game has been running for {Round.ElapsedTime.TotalSeconds}s.";
                    response = Translation.GlobalKitTimeout.Replace("%timeout%",kit.GlobalKitTimeout.ToString()).Replace("%runningtime%",Round.ElapsedTime.TotalSeconds.ToString());
                    return false;
                }
            }
        }
        
        if (kit.InitialGlobalCooldown > 0)
        {
            if (kit.InitialGlobalCooldown > Round.ElapsedTime.TotalSeconds)
            {
                if (!player.CheckPermission("kits.give.cooldownbypass"))
                {
                    //response = $"This kit cannot be redeemed for {kit.InitialGlobalCooldown}s after the game has started. The game has been running for {Round.ElapsedTime.TotalSeconds}s.";
                    response = Translation.GlobalCooldown.Replace("%cooldown%",kit.InitialGlobalCooldown.ToString()).Replace("%runningtime%",Round.ElapsedTime.TotalSeconds.ToString());
                    return false;
                }
            }
        }
        
        if (kit.SpawnKitTimeout > 0)
        {
            if (Round.ElapsedTime.TotalSeconds >
                kit.SpawnKitTimeout + Plugin.Instance.KitManager.PlayerSpawnTime[player])
            {
                if (!player.CheckPermission("kits.give.timeoutbypass"))
                {
                    if(Plugin.Instance.Config.Debug)Log.Debug($"Player spawn time: {Plugin.Instance.KitManager.PlayerSpawnTime[player]}s Timeout should be after {Plugin.Instance.KitManager.PlayerSpawnTime[player] + kit.SpawnKitTimeout}s. Current time: {Round.ElapsedTime.TotalSeconds}s");
                    //response = $"This kit cannot be redeemed after {kit.SpawnKitTimeout}s after spawning! You have been alive for: {Round.ElapsedTime.TotalSeconds - Plugin.Instance.KitManager.PlayerSpawnTime[player]}s";
                    response = Translation.SpawnKitTimeout.Replace("%timeout%",kit.SpawnKitTimeout.ToString()).Replace("%alive%",(Round.ElapsedTime.TotalSeconds - Plugin.Instance.KitManager.PlayerSpawnTime[player]).ToString());
                    return false;
                }
            }
        }

        if (!player.CheckPermission("kits.give.givebypass"))
        {
            if (kit.WhitelistedRoles != null)
            {
                if (!kit.WhitelistedRoles.Contains(player.Role))
                {
                    //response = $"This player may not recieve this kit as {player.Role.Name}";
                    response = Translation.RecieveRole.Replace("%role%", player.Role.Name);
                    return false;
                }
            }

            if (kit.BlacklistedRoles != null)
            {
                if (kit.BlacklistedRoles.Contains(player.Role))
                {
                    //response = $"This player may not recieve this kit as {player.Role.Name}";
                    response = Translation.RecieveRole.Replace("%role%", player.Role.Name);
                    return false;
                }
            }
        }
        
        if (kit.OverrideInventory == true)
        {
            if (kit.DropOverridenItems)
            {
                player.DropItems();
            }
            else
            {
                player.ClearInventory(true);
            }
        }
        
        if (kit.MaxUses > 0)
        {
            KitUseEntry useEntry = Plugin.Instance.KitManager.GetKitUseEntry(player, kit);
            if (useEntry != null)
            {
                int uses = useEntry.Uses;
                if (uses >= kit.MaxUses)
                {
                    if (!player.CheckPermission("kits.give.givebypass"))
                    {
                        //response = $"You have already used this kit {uses} times. You cannot use it more than {kit.MaxUses} times.";
                        response = Translation.MaxUses.Replace("%times%",uses.ToString()).Replace("%maxuses%",kit.MaxUses.ToString());
                        return false;
                    }
                }
                else
                {
                    if(Plugin.Instance.Config.Debug)Log.Debug($"kit entry uses for {player.Nickname} are {uses}");
                    //Plugin.Instance.KitManager.KitUseEntries.Find(x => x.Player == player && x.KitEntry == kit).Uses++;
                    useEntry.Uses += 1;
                }
            }
            else
            {
                if(Plugin.Instance.Config.Debug)Log.Debug($"Adding kit entry uses for {player.Nickname} with 1");
                Plugin.Instance.KitManager.KitUseEntries.Add(new KitUseEntry(kit,player,1));
            }
        }
        
        Plugin.Instance.KitEntryManager.GiveKitContents(player, kit);
        //response = $"Gave kit {kit.Name} to {player.Nickname}";
        response = Translation.GiveKit.Replace("%kit%",kit.Name).Replace("%player%",player.Nickname);

        if (!((CommandSender)sender).CheckPermission("kits.give.cooldownbypass"))
        {
            if (kit.CooldownInSeconds > -1f || kit.InitialCooldown > -1f)
            {
                Plugin.Instance.KitManager.StartKitCooldown(kit, player, kit.CooldownInSeconds);
                if (Plugin.Instance.Config.Debug) Log.Debug($"Starting kit cooldown for {kit.Name} for {kit.CooldownInSeconds} seconds");
            }
        }
        
        
        return true;
    }
}
// seperating them sucks but it works i guess should make a combination or whatever its called lmao
[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Kit : ICommand
{
    public string Command { get; } = "kit";
    public string[] Aliases { get; } = new[] { "claimkit" };
    public string Description { get; } = "Claims a kit";
    private static Translation Translation => Plugin.Instance.Translation;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = "Entered too little arguments. Usage: kits kit (kit name)";
            return false;
        }
        
        if (Plugin.Instance.KitEntryManager == null)
        {
            response = "Internal error. (Kit manager instance is null)";
            return false;
        }

        var kitNameArg = arguments.At(0);
        if (Plugin.Instance.KitEntryManager.GetKitEntryFromName(kitNameArg) == null)
        {
            //response = "Kit with specified name could not be found";
            response = Translation.InvalidKitName;
            return false;
        }
        
        KitEntry kit = Plugin.Instance.KitEntryManager.GetKitEntryFromName(kitNameArg);
        
        if (kit != null)
        {
            // When kit use permission is enabled, check if sender can recieve this kit
            if (kit.UsePermission)
            {
                if (!((CommandSender)sender).CheckPermission($"kits.give.{kit.Name}"))
                {
                    //response = $"You do not have permission to redeem this kit (kits.give.{kit.Name})";
                    response = Translation.InvalidKitPermission.Replace("%permission%", $"kits.give.{kit.Name}");
                    return false;
                }
            }
            else
            {
                if (!((CommandSender)sender).CheckPermission("kits.give"))
                {
                    //response = "You do not have permission (kits.give) to execute this command.";
                    response = Translation.InvalidGivePermissions;
                    return false;
                }
            }
        }
        else
        {
            response = "<color=red>ERROR: Kit entry is null even though that should be impossible?!?!</color>";
            return false;
        }
        
        Player player;
        
        try
        {
            player = Player.Get(sender);
        }
        catch (Exception e)
        {
            Log.Error($"Player sender could not be parsed! Exception: {e}");
            response = "<color=red>ERROR: Sender player could not be parsed!</color>";
            return false;
        }
        
        if (!Round.IsStarted)
        {
            if (kit.WhitelistedRoles == null)
            {
                if (!player.CheckPermission("kits.give.givebypass"))
                {
                    //response = "Kits cannot be redeemed before the round has started.";
                    response = Translation.KitRoundStart;
                    return false;
                }
            }
            else
            {
                if (kit.WhitelistedRoles.Contains(RoleTypeId.Scientist))
                {
                    if (!player.CheckPermission("kits.give.givebypass"))
                    {
                        //response = "Kits cannot be redeemed before the round has started.";
                        response = Translation.KitRoundStart;
                        return false;
                    }
                }
            }
        }
        
        if (!kit.Enabled && !((CommandSender)sender).CheckPermission("kits.give.givebypass"))
        {
            //response = "This kit is not enabled and cannot be redeemed.";
            response = Translation.KitNotEnabled;
            return false;
        }
        
        if (kit.CooldownInSeconds > -1f || kit.InitialCooldown > -1f)
        {
            if (Plugin.Instance.KitManager.IsKitEntryOnCooldown(kit,player))
            {
                if (!((CommandSender)sender).CheckPermission("kits.give.cooldownbypass"))
                {
                    CooldownEntry cooldownEntry = Plugin.Instance.KitManager.GetCooldownEntry(player, kit);
                    //response = $"This kit is on cooldown for {cooldownEntry.RemainingTime}s";
                    response = Translation.KitCooldown.Replace("%cooldown%",cooldownEntry.RemainingTime.ToString());
                    return false;
                }
            }
        }

        if (kit.GlobalKitTimeout > 0)
        {
            if (kit.GlobalKitTimeout < Round.ElapsedTime.TotalSeconds)
            {
                if (!player.CheckPermission("kits.give.timeoutbypass"))
                {
                    //response = $"You cannot use this kit after {kit.GlobalKitTimeout}s of the game starting. The game has been running for {Round.ElapsedTime.TotalSeconds}s.";
                    response = Translation.GlobalKitTimeout.Replace("%timeout%",kit.GlobalKitTimeout.ToString()).Replace("%runningtime%",Round.ElapsedTime.TotalSeconds.ToString());
                    return false;
                }
            }
        }
        
        if (kit.InitialGlobalCooldown > 0)
        {
            if (kit.InitialGlobalCooldown > Round.ElapsedTime.TotalSeconds)
            {
                if (!player.CheckPermission("kits.give.cooldownbypass"))
                {
                    //response = $"This kit cannot be redeemed for {kit.InitialGlobalCooldown}s after the game has started. The game has been running for {Round.ElapsedTime.TotalSeconds}s.";
                    response = Translation.GlobalCooldown.Replace("%cooldown%",kit.InitialGlobalCooldown.ToString()).Replace("%runningtime%",Round.ElapsedTime.TotalSeconds.ToString());
                    return false;
                }
            }
        }
        
        if (kit.SpawnKitTimeout > 0)
        {
            if (Round.ElapsedTime.TotalSeconds >
                kit.SpawnKitTimeout + Plugin.Instance.KitManager.PlayerSpawnTime[player])
            {
                if (!player.CheckPermission("kits.give.timeoutbypass"))
                {
                    if(Plugin.Instance.Config.Debug)Log.Debug($"Player spawn time: {Plugin.Instance.KitManager.PlayerSpawnTime[player]}s Timeout should be after {Plugin.Instance.KitManager.PlayerSpawnTime[player] + kit.SpawnKitTimeout}s. Current time: {Round.ElapsedTime.TotalSeconds}s");
                    //response = $"This kit cannot be redeemed after {kit.SpawnKitTimeout}s after spawning! You have been alive for: {Round.ElapsedTime.TotalSeconds - Plugin.Instance.KitManager.PlayerSpawnTime[player]}s";
                    response = Translation.SpawnKitTimeout.Replace("%timeout%",kit.SpawnKitTimeout.ToString()).Replace("%alive%",(Round.ElapsedTime.TotalSeconds - Plugin.Instance.KitManager.PlayerSpawnTime[player]).ToString());
                    return false;
                }
            }
        }

        if (!player.CheckPermission("kits.give.givebypass"))
        {
            if (kit.WhitelistedRoles != null)
            {
                if (!kit.WhitelistedRoles.Contains(player.Role))
                {
                    //response = $"You may not recieve this kit as {player.Role.Name}";
                    response = Translation.RecieveRole.Replace("%role%", player.Role.Name);
                    return false;
                }
            }

            if (kit.BlacklistedRoles != null)
            {
                if (kit.BlacklistedRoles.Contains(player.Role))
                {
                    //response = $"You may not recieve this kit as {player.Role.Name}";
                    response = Translation.RecieveRole.Replace("%role%", player.Role.Name);
                    return false;
                }
            }
        }
        
        if (kit.OverrideInventory == true)
        {
            if (kit.DropOverridenItems)
            {
                player.DropItems();
            }
            else
            {
                player.ClearInventory(true);
            }
        }

        if (kit.MaxUses > 0)
        {
            KitUseEntry useEntry = Plugin.Instance.KitManager.GetKitUseEntry(player, kit);
            if (useEntry != null)
            {
                int uses = useEntry.Uses;
                if (uses >= kit.MaxUses)
                {
                    if (!player.CheckPermission("kits.give.givebypass"))
                    {
                        //response = $"You have already used this kit {uses} times. You cannot use it more than {kit.MaxUses} times.";
                        response = Translation.MaxUses.Replace("%times%",uses.ToString()).Replace("%maxuses%",kit.MaxUses.ToString());
                        return false;
                    }
                }
                else
                {
                    if(Plugin.Instance.Config.Debug)Log.Debug($"kit entry uses for {player.Nickname} are {uses}");
                    //Plugin.Instance.KitManager.KitUseEntries.Find(x => x.Player == player && x.KitEntry == kit).Uses++;
                    useEntry.Uses += 1;
                }
            }
            else
            {
                if(Plugin.Instance.Config.Debug)Log.Debug($"Adding kit entry uses for {player.Nickname} with 1");
                Plugin.Instance.KitManager.KitUseEntries.Add(new KitUseEntry(kit,player,1));
            }
        }
        
        Plugin.Instance.KitEntryManager.GiveKitContents(player, kit);
        //response = $"Gave kit {kit.Name} to you!";
        response = Translation.KitRedeemText.Replace("%kit%", kit.Name);

        if (!((CommandSender)sender).CheckPermission("kits.give.cooldownbypass"))
        {
            if (kit.CooldownInSeconds > -1f || kit.InitialCooldown > -1f)
            {
                Plugin.Instance.KitManager.StartKitCooldown(kit, player, kit.CooldownInSeconds);
                if (Plugin.Instance.Config.Debug) Log.Debug($"Starting kit cooldown for {kit.Name} for {kit.CooldownInSeconds} seconds");
            }
        }
        
        
        return true;
    }
}