using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace InfiniteWeapons
{
    public partial class InfiniteWeapons : BasePlugin
    {
        public override string ModuleName => "CS2 InfiniteWeapons";
        public override string ModuleAuthor => "Kalle <kalle@kandru.de>";

        public MemoryFunctionWithReturn<CCSPlayer_WeaponServices, int, int, IntPtr>? WeaponServicesGetSlot_Func { get; set; }

        public override void Load(bool hotReload)
        {
            // update gamedata
            UpdateGamedata();
            // init WeaponServicesGetSlot
            try
            {
                // get function
                WeaponServicesGetSlot_Func = new MemoryFunctionWithReturn<CCSPlayer_WeaponServices, int, int, IntPtr>(GameData.GetSignature(_signatureName));
                // register hook
                WeaponServicesGetSlot_Func.Hook(OnWeaponGetSlot, HookMode.Post);
                // check if function is valid
                Console.WriteLine(Localizer["core.init"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Localizer["core.failed.init"].Value.Replace("{error}", ex.Message));
                WeaponServicesGetSlot_Func = null;
            }
        }

        public override void Unload(bool hotReload)
        {
            if (WeaponServicesGetSlot_Func == null) return;
            WeaponServicesGetSlot_Func.Unhook(OnWeaponGetSlot, HookMode.Post);
        }

        private HookResult OnWeaponGetSlot(DynamicHook hook)
        {
            // disable hook if not enabled
            if (!Config.Enabled) return HookResult.Continue;
            // get parameters
            CCSPlayer_WeaponServices? weaponServices = hook.GetParam<CCSPlayer_WeaponServices>(0);
            int slot = hook.GetParam<int>(1);
            if (weaponServices == null
                || weaponServices.Pawn == null
                || weaponServices.Pawn.Value == null
                || weaponServices.Pawn.Value.Controller == null
                || !weaponServices.Pawn.Value.Controller.IsValid
                || weaponServices.Pawn.Value.Controller.Value == null
                || slot < 0
                || slot > 1) return HookResult.Continue;
            CCSPlayerController? player = weaponServices.Pawn.Value.Controller.Value.As<CCSPlayerController>();
            if (player == null || !player.IsValid) return HookResult.Continue;

            // If no permissions are set, we continue (everyone allowed)
            if (Config.Permissions.Count == 0
                && Config.PermissionsIsWhitelist)
            {
                DebugPrint($"No permissions set and whitelist is enabled, infinite weapons not allowed");
                return HookResult.Continue;
            }
            // Check if player has at least one permission from the list
            bool hasPermission = false;
            int MaxPrimaryWeapons = Config.MaxPrimaryWeapons;
            int MaxSecondaryWeapons = Config.MaxSecondaryWeapons;
            foreach (var kvp in Config.Permissions)
            {
                if (AdminManager.PlayerHasPermissions(player, kvp.Key))
                {
                    DebugPrint($"Player {player.PlayerName} has permission {kvp.Key} to have {kvp.Value.MaxPrimaryWeapons} primary and {kvp.Value.MaxSecondaryWeapons} secondary weapons");
                    MaxPrimaryWeapons = kvp.Value.MaxPrimaryWeapons;
                    MaxSecondaryWeapons = kvp.Value.MaxSecondaryWeapons;
                    hasPermission = true;
                    break;
                }
            }
            // If the player has no permission and the list is a whitelist, we return
            if (Config.PermissionsIsWhitelist && !hasPermission)
            {
                DebugPrint($"Player {player.PlayerName} has no permission to use infinite weapons");
                return HookResult.Continue;
            }
            // count players current weapons
            int weaponCount = weaponServices.MyWeapons
                .Where(weapon =>
                    weapon != null
                    && weapon.IsValid
                    && weapon.Value != null
                    && weapon.Value.IsValid
                    && weapon.Value.VData != null)
                .Count(weapon => weapon!.Value!.As<CCSWeaponBase>().VData!.GearSlot == (gear_slot_t)slot) + 1;
            // check if one more weapon is allowed
            if (slot == 0 && weaponCount <= MaxPrimaryWeapons
                || slot == 1 && weaponCount <= MaxSecondaryWeapons)
            {
                // set return value to 0 to allow another weapon to be picked up
                hook.SetReturn(IntPtr.Zero);
                DebugPrint($"Player {player.PlayerName} Slot {slot} has now {weaponCount} weapons");
            }
            return HookResult.Continue;
        }
    }
}