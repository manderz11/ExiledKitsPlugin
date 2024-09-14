# Kits

A Exiled kits plugin for SCP:SL. This plugin supports creation of kits only through the config file. Kits support giving items, setting roles, giving ammo and status effects with configurable intensities and durations.

## Installation

- Download a release in the [releases](https://github.com/manderz11/ExiledKitsPlugin/releases) tab.

- Place the plugin .dll file inside EXILED/Plugins

- Start the server

- Profit

## Configuring

Inside EXILED/Configs edit your servers (port)-config.yml

Under kits edit the configurations to your desires.

Examples are provided as of the latest aviable version, including pre-releases.

Example kit (generated from initial plugin launch):
```
kits:
  kits:
  - name: 'Example'
    enabled: true
    # Should the kit be permissions based
    use_permission: true
    # Initial cooldown of the kit after player spawn, set to 0 to disable
    initial_cooldown: 0
    # Initial cooldown of the kit after game start, set to 0 to disable
    initial_global_cooldown: 30
    # Time after game start to timeout kit (disable redeeming), set to 0 to disable
    global_kit_timeout: 180
    # Time after player spawn to timeout kit (disable redeeming), set to 0 to disable
    spawn_kit_timeout: 0
    # Kit re-use cooldown in seconds, set to 0 to disable
    cooldown_in_seconds: 30
    # Maximum number of uses per game, set to 0 to disable
    max_uses: 2
    # Allowed roles to redeem kit
    whitelisted_roles:
    - Scientist
    - FacilityGuard
    # Blacklisted roles to redeem kit, overrides whitelisted if the same role is set in whitelisted
    blacklisted_roles: 
    # Should kit delete existing inventory items
    override_inventory: false
    # Should overriden items be destroyed
    drop_overriden_items: true
    items:
    - Adrenaline
    - Medkit
    - Lantern
    - GunCOM15
    ammo: 
    effects:
    -
    # The effect type
      type: Scp207
      # The effect duration
      duration: 0
      # The effect intensity
      intensity: 1
      # If the effect is already active, setting to true will add this duration onto the effect.
      add_duration_if_active: false
      # Indicates whether the effect should be enabled or not
      is_enabled: true
    set_role: NtfCaptain
```

## Permissions

- kits.give, kits.give.(kit name if usepermission is true),
- kits.enable, kits.disable,
- kits.delete,
- kits.debug,
- kits.give.givebypass (if disabled), kits.give.cooldownbypass, kits.give.timeoutbypass

## TO-DO

- Saving kit configuration changes to file
- In-game kit editing

## Known issues

Known issues as of the latest release, including pre-releases

- Known permissions issue after adding the plugin with a new permission entry, usually after a update, a server restart is required to apply permission entries to be valid for permissions. This will be possibly fixed in the next full release as the plugin does not register permissions but instead checks with has permission method.

## Support

Support for the plugin can only be found on the [issues](https://github.com/manderz11/ExiledKitsPlugin/issues) tab or on my [community discord server](https://discord.gg/ZWsQkf689J) in the plugins and mods category

## Suggestions

Submission for suggestions can also be found on the [discussions](https://github.com/manderz11/ExiledKitsPlugin/discussions) tab by creating a new discussion with ideas tag or also on my [community discord server](https://discord.gg/ZWsQkf689J) in the plugins and mods category

## Special thanks

Special thanks to:
- Rasnirt (rasnirt) on discord for suggesting features for the plugin on discord.
