using CounterStrikeSharp.API.Core;
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
                || slot < 0
                || slot > 1) return HookResult.Continue;
            int weaponCount = weaponServices.MyWeapons
                .Where(weapon =>
                    weapon != null
                    && weapon.IsValid
                    && weapon.Value != null
                    && weapon.Value.IsValid
                    && weapon.Value.VData != null)
                .Count(weapon => weapon!.Value!.As<CCSWeaponBase>().VData!.GearSlot == (gear_slot_t)slot) + 1;
            if (slot == 0 && weaponCount <= Config.MaxPrimaryWeapons
                || slot == 1 && weaponCount <= Config.MaxSecondaryWeapons)
            {
                // set return value to 0
                hook.SetReturn(IntPtr.Zero);
                DebugPrint($"Slot {slot} has now {weaponCount} weapons");
            }
            return HookResult.Continue;
        }
    }
}