using System.ComponentModel;
using Exiled.API.Interfaces;

namespace ExiledKitsPlugin;

public class Translation : ITranslation
{
    [Description("Text shown when a kit is redeemed")]
    public string KitRedeemText { get; set; } = "You have redeemed the %kit% kit!";
    
    [Description("Text shown when a kit is given")]
    public string GiveKit { get; set; } = "Gave %kit% to %player%";

    [Description("Text shown when kit could not be found with the name provided")]
    public string InvalidKitName { get; set; } = "Kit with specified name could not be found!";
    
    [Description("Text shown when the player does not have permission to execute this command")]
    public string InvalidGivePermissions { get; set; } = "You do not have permission kits.give to execute this command!";
    
    [Description("Text shown when the player does not have permission to redeem the kit")]
    public string InvalidKitPermission { get; set; } = "You do not have permission %permission% to redeem this kit!";
    
    [Description("Text shown when the kit cannot be redeemed before the round has started")]
    public string KitRoundStart { get; set; } = "Kits cannot be redeemed before the round has started.";
    
    [Description("Text shown when the kit is not enabled")]
    public string KitNotEnabled { get; set; } = "This kit is not enabled and cannot be redeemed";
    
    [Description("Text shown when the kit is on cooldown")]
    public string KitCooldown { get; set; } = "This kit is on cooldown for %cooldown%s";
    
    [Description("Text shown when the kit cannot be redeemed after x seconds of the game starting (global timeout)")]
    public string GlobalKitTimeout{ get; set; } = "You cannot use this kit after %timeout% seconds of the game starting. The game has been running for %runningtime% seconds.";
    
    [Description("Text shown when the kit cannot be redeemed before x seconds of the game starting (initial global cooldown)")]
    public string GlobalCooldown { get; set; } = "This kit cannot be redeemed for %cooldown% seconds after the game starts. The game has been running for %runningtime% seconds.";
    
    [Description("Text shown when the kit cannot be redeemed after x seconds of the player spawning (spawn kit timeout)")]
    public string SpawnKitTimeout { get; set; } = "This kit cannot be redeemed after %timeout%s after spawning. You have been alive for %alive%s";
    
    [Description("Text shown when the kit cannot be redeemed as a role")]
    public string RecieveRole { get; set; } = "You may not recieve this role as %role%.";
    
    [Description("Text shown when the player has used the kit too many times")]
    public string MaxUses { get; set; } = "You have already used this kit %times%. You cannot use it more than %maxuses% times.";
    
    [Description("Text shown for the parent help command")]
    public string ParentCommand { get; set; } = "Use: kits (list | give | delete | enable | disable | debug)";
    
}