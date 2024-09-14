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
    use_permission: true
    initial_cooldown: 15
    initial_global_cooldown: 0
    global_kit_timeout: 180
    cooldown_in_seconds: 30
    max_uses: 2
    whitelisted_roles:
    - Scientist
    - FacilityGuard
    blacklisted_roles: 
    override_inventory: false
    drop_overriden_items: true
    items:
    - Adrenaline
    - Medkit
    - Lantern
    - GunCOM15
    ammo: 
    effects:
    -
      type: Scp207
      duration: 0
      intensity: 1
      add_duration_if_active: false
      is_enabled: true
    set_role: NtfCaptain
```

## Permissions

- kits.give, kits.give.(kit name if usepermission is true),
- kits.enable, kits.disable,
- kits.delete,
- kits.debug,
- kits.give.givebypass (if disabled), kits.give.cooldownbypass, kits.give.timoutbypass

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
