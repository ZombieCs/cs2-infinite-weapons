using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Extensions;
using System.Text.Json.Serialization;

namespace InfiniteWeapons
{
    public class PlayerPermissions
    {
        // maximum primary weapons
        [JsonPropertyName("max_primary_weapons")] public int MaxPrimaryWeapons { get; set; } = 2;
        // maximum secondary weapons
        [JsonPropertyName("max_secondary_weapons")] public int MaxSecondaryWeapons { get; set; } = 2;
    }

    public class PluginConfig : BasePluginConfig
    {
        // Enabled or disabled
        [JsonPropertyName("enabled")] public bool Enabled { get; set; } = true;
        // debug prints
        [JsonPropertyName("debug")] public bool Debug { get; set; } = false;
        // file to save signature too
        [JsonPropertyName("signature_file")] public string SignatureFile { get; set; } = "InfiniteWeapons.json";
        // allow only players with this permission(s) to have infinite weapons. Overrides definitions below
        [JsonPropertyName("permissions")] public Dictionary<string, PlayerPermissions> Permissions { get; set; } = new Dictionary<string, PlayerPermissions>();
        // whether or not only players with the permission can use infinite weapons
        [JsonPropertyName("permissions_is_whitelist")] public bool PermissionsIsWhitelist { get; set; } = false;
        // maximum primary weapons
        [JsonPropertyName("max_primary_weapons")] public int MaxPrimaryWeapons { get; set; } = 2;
        // maximum secondary weapons
        [JsonPropertyName("max_secondary_weapons")] public int MaxSecondaryWeapons { get; set; } = 2;
        // override signature windows
        [JsonPropertyName("override_signature_windows")] public string OverrideSignatureWindows { get; set; } = "";
        // override signature linux
        [JsonPropertyName("override_signature_linux")] public string OverrideSignatureLinux { get; set; } = "";
    }

    public partial class InfiniteWeapons : BasePlugin, IPluginConfig<PluginConfig>
    {
        public required PluginConfig Config { get; set; }

        public void OnConfigParsed(PluginConfig config)
        {
            Config = config;
            // update config and write new values from plugin to config file if changed after update
            Config.Update();
            Console.WriteLine(Localizer["core.config"]);
        }
    }
}
