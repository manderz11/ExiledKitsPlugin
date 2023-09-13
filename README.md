# Kits

A Exiled kits plugin for SCP:SL. This plugin supports creation of kits only through the config file. Kits support giving items, ammo and status effects with configurable intensities and durations.

## Installation

-Download a release in the [release](https://github.com/manderz11/ExiledKitsPlugin/releases) tab.

-Place the plugin .dll file inside EXILED/Plugins

-Start the server

-Profit

## Configuring

Inside EXILED/Configs edit your servers (port)-config.yml

Under kits edit the configurations to your desires

Example kit:
```
kits:
  kits:
  - name: 'example'
    enabled: true
    use_permission: false
    cooldown_in_seconds: 60
    items:
    - Medkit
    - Coin
    - Flashlight
    ammo:
      Nato9: 60
      Nato556: 15
    effects:
    -
      type: MovementBoost
      duration: 300
      intensity: 100
      add_duration_if_active: false
      is_enabled: true
```

## Permissions

- kits.give, kits.give.(kit name if usepermission is true),
- kits.enable, kits.disable,
- kits.delete,
- kits.givebypass (if disabled), kits.give.cooldownbypass 

## TO-DO

- Saving kit configuration changes to file
- In-game kit editing

## Support

Support for the plugin can only be found on the [issues](https://github.com/manderz11/ExiledKitsPlugin/issues) tab or by messaging me on discord (@manderz11)
