using System;
using BepInEx.Configuration;
using MDSFramework.Api;

namespace VanguardDefence
{
    internal static class VanguardConfig
    {
        internal static ConfigEntry<bool> ModEnabled { get; private set; } = null!;
        internal static ConfigEntry<string> DefaultFaction { get; private set; } = null!;
        internal static ConfigEntry<bool> UseAnimatedIsoContainers { get; private set; } = null!;
        internal static ConfigEntry<bool> PreferLaserBeforeMissiles { get; private set; } = null!;
        internal static ConfigEntry<float> HelLaserDwellSeconds { get; private set; } = null!;
        internal static ConfigEntry<float> AesaJamTolerance { get; private set; } = null!;
        internal static ConfigEntry<bool> EnableTwoFcuBlinking { get; private set; } = null!;
        internal static ConfigEntry<float> AutoPopUpRangeKm { get; private set; } = null!;
        internal static ConfigEntry<string> CustomAssetRootOverride { get; private set; } = null!;

        internal static void Initialize(ConfigFile config)
        {
            ModEnabled = config.Bind(
                "01 General",
                "ModEnabled",
                true,
                "Enable Vanguard Defence Skyshield contractor package.");

            DefaultFaction = config.Bind(
                "01 General",
                "DefaultFaction",
                "Boscali",
                new ConfigDescription(
                    "Faction affinity for Vanguard Defence hardware (Boscali = Western default, Any = all factions, Primeva = Eastern override).",
                    new AcceptableValueList<string>("Boscali", "Any", "Primeva")));

            UseAnimatedIsoContainers = config.Bind(
                "02 Skyshield Hardware",
                "UseAnimatedIsoContainers",
                true,
                "Use animated stealth 20ft DIN ISO 668 clamshell container models instead of open deployed pallets.");

            PreferLaserBeforeMissiles = config.Bind(
                "02 Skyshield Hardware",
                "PreferLaserBeforeMissiles",
                true,
                "Assign Skyshield HEL laser as cascade priority 1 before expending SAM ammunition.");

            HelLaserDwellSeconds = config.Bind(
                "02 Skyshield Hardware",
                "HelLaserDwellSeconds",
                1.65f,
                new ConfigDescription(
                    "Base HEL laser dwell time in seconds against unhardened targets (multiplied by ThermalHardening).",
                    new AcceptableValueRange<float>(0.3f, 5.0f)));

            AesaJamTolerance = config.Bind(
                "02 Skyshield Hardware",
                "AesaJamTolerance",
                0.78f,
                new ConfigDescription(
                    "AESA jamming tolerance on Skyshield FCU radar (0.78 base, boosted +0.10 to 0.88 inside Skyshield.net).",
                    new AcceptableValueRange<float>(0.3f, 0.98f)));

            EnableTwoFcuBlinking = config.Bind(
                "02 Skyshield Hardware",
                "EnableTwoFcuBlinking",
                true,
                "Enable staggered 2-FCU radar blinking and auto-handoff against ARAD and standoff jammers.");

            AutoPopUpRangeKm = config.Bind(
                "02 Skyshield Hardware",
                "AutoPopUpRangeKm",
                28f,
                new ConfigDescription(
                    "Range in km at which stowed Skyshield ISO containers automatically pop open.",
                    new AcceptableValueRange<float>(5f, 80f)));

            CustomAssetRootOverride = config.Bind(
                "03 Assets",
                "CustomAssetRootOverride",
                string.Empty,
                "Optional custom directory override containing Skyshield .glb files and branding/ decals.");

            DefaultFaction.SettingChanged += OnLiveConfigChanged;
            PreferLaserBeforeMissiles.SettingChanged += OnLiveConfigChanged;
            HelLaserDwellSeconds.SettingChanged += OnLiveConfigChanged;
            AesaJamTolerance.SettingChanged += OnLiveConfigChanged;
            EnableTwoFcuBlinking.SettingChanged += OnLiveConfigChanged;
            AutoPopUpRangeKm.SettingChanged += OnLiveConfigChanged;
        }

        private static void OnLiveConfigChanged(object sender, EventArgs e)
        {
            VanguardPackageBuilder.ApplyLiveConfig(VanguardPlugin.Instance?.RegisteredPackage);
        }

        internal static FactionAffinity ParseConfiguredFaction()
        {
            string val = DefaultFaction.Value ?? "Boscali";
            if (string.Equals(val, "Any", StringComparison.OrdinalIgnoreCase))
            {
                return FactionAffinity.Any;
            }

            if (string.Equals(val, "Primeva", StringComparison.OrdinalIgnoreCase))
            {
                return FactionAffinity.Primeva;
            }

            return FactionAffinity.Boscali;
        }
    }
}
