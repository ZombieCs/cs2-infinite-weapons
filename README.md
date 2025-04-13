# CounterstrikeSharp - Infinite Weapons

[![UpdateManager Compatible](https://img.shields.io/badge/CS2-UpdateManager-darkgreen)](https://github.com/Kandru/cs2-update-manager/)
[![GitHub release](https://img.shields.io/github/release/Kandru/cs2-infinite-weapons?include_prereleases=&sort=semver&color=blue)](https://github.com/Kandru/cs2-infinite-weapons/releases/)
[![License](https://img.shields.io/badge/License-GPLv3-blue)](#license)
[![issues - cs2-infinite-weapons](https://img.shields.io/github/issues/Kandru/cs2-infinite-weapons)](https://github.com/Kandru/cs2-infinite-weapons/issues)
[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate/?hosted_button_id=C2AVYKGVP9TRG)

This is a simple Infinite Weapon plug-in which allows a player to carry more then one primary or secondary weapon. Hint: this plug-in will likely break with any CS2 update because it uses modified gamedata. If the plug-in is not working anymore please check this repository first and open a Github Issue if a newer version does not solve your issue.

This plug-in automatically updates the gamedata.json of your CounterstrikeSharp installation for you! Therefore please restart the game server after installing/updating the plug-in and starting the game server.

## Installation

1. Download and extract the latest release from the [GitHub releases page](https://github.com/Kandru/cs2-infinite-weapons/releases/).
2. Move the "InfiniteWeapons" folder to the `/addons/counterstrikesharp/plugins/` directory.
3. (Re)start the server and wait for it to be completely loaded.
4. Restart the server again because it maybe applied some Gamedata entries for the plug-in to work correctly.

Updating is even easier: simply overwrite all plugin files and they will be reloaded automatically. To automate updates please use our [CS2 Update Manager](https://github.com/Kandru/cs2-update-manager/).


## Configuration

This plugin automatically creates a readable JSON configuration file. This configuration file can be found in `/addons/counterstrikesharp/configs/plugins/InfiniteWeapons/InfiniteWeapons.json`.

```json
{
  "enabled": true,
  "debug": true,
  "max_primary_weapons": 2,
  "max_secondary_weapons": 2,
  "override_signature_windows": "",
  "override_signature_linux": "",
  "ConfigVersion": 1
}
```

### enabled

Whether this plug-in is enabled or not.

### debug

Debug mode. Only necessary during development or testing.

### max_primary_weapons

Maximum number of primary weapons. More then 2 are possible but you cannot easily switch between them.

### max_secondary_weapons

Maximum number of secondary weapons. More then 2 are possible but you cannot easily switch between them.

### override_signature_windows

When the signature changes and no new release is available you can override the signature yourself. Make sure to remove the signature when updating this plug-in because this settings overrides the signatures that come with the plug-in.

### override_signature_linux

When the signature changes and no new release is available you can override the signature yourself. Make sure to remove the signature when updating this plug-in because this settings overrides the signatures that come with the plug-in.

## Commands

### infiniteweapons (Server Console Only)

Ability to run sub-commands:

```
infiniteweapons update
infiniteweapons reload
infiniteweapons disable
infiniteweapons enable
```

#### update

This reloads the configuration from the disk and updates the signature in the gamedata.json file with the values from the config file (if not empty). Otherwise it overwrites it with the integrated last-known signature.

#### reload

Reloads the configuration.

#### disable

Disables the plug-in instantly and remembers this state.

#### enable

Enables the plug-in instantly and remembers this state.

## Compile Yourself

Clone the project:

```bash
git clone https://github.com/Kandru/cs2-infinite-weapons.git
```

Go to the project directory

```bash
  cd cs2-infinite-weapons
```

Install dependencies

```bash
  dotnet restore
```

Build debug files (to use on a development game server)

```bash
  dotnet build
```

Build release files (to use on a production game server)

```bash
  dotnet publish
```

## FAQ

TBD

## License

Released under [GPLv3](/LICENSE) by [@Kandru](https://github.com/Kandru).

## Authors

- [@derkalle4](https://www.github.com/derkalle4)
