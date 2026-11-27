using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MDSFramework.Api;
using MDSFramework.Networking;

namespace VanguardDefence
{
    internal static class VanguardPackageBuilder
    {
        internal const string PackageId = "vanguard.skyshield";
        internal const string SkyDartSrSamMissileId = "vgd_skydart_srsam";
        internal const string EccmHunterMissileId = "vgd_eccm_hunter";
        internal const string IcbmHunterMissileId = "vgd_icbm_hunter";
        internal const string SkyLanceLrSamMissileId = "vgd_skylance_lrsam";
        internal const string SkyshieldVlsUnitId = "vgd_skyshield_vls";
        internal const string LeitstandUnitId = "vgd_skyshield_c2";
        internal const string SeaWraithMissileId = "vgd_seawraith";
        internal const string SeaWraithContainerUnitId = "vgd_seawraith_container";
        internal const string WaspAtMissileId = "vgd_wasp_at";
        internal const string WaspHiveContainerUnitId = "vgd_wasp_hive_container";
        internal const string Hx3IsoCarrierUnitId = "vgd_hx3_iso_carrier";

        internal static ContractorPackage Build(string pluginDirectory, out string assetSummary)
        {
            string assetRoot = VanguardAssetResolver.ResolveAssetRoot(pluginDirectory);
            string brandingDir = Path.Combine(assetRoot, "branding");
            bool useContainers = VanguardConfig.UseAnimatedIsoContainers.Value;

            var wtaProfile = new SmartEngagementProfile
            {
                ProfileId = "west_precision_skyshield",
                IdleTickHz = 1f,
                ActiveTickHz = 4f,
                PreferLaserBeforeMissiles = VanguardConfig.PreferLaserBeforeMissiles.Value,
                LaserDwellSeconds = VanguardConfig.HelLaserDwellSeconds.Value,
                UseSalvoDeconfliction = true,
                MaxSimultaneousPerThreat = 2,
                AutoPopUpRangeKm = VanguardConfig.AutoPopUpRangeKm.Value,
                StowAfterIdleSeconds = 16f,
                DeprioritizeDetectedDecoys = true,
                CascadePriority = new[]
                {
                    WeaponCascadeRole.LaserHel,
                    WeaponCascadeRole.CloseInGun,
                    WeaponCascadeRole.ShortMedSam,
                    WeaponCascadeRole.LongRangeSam,
                    WeaponCascadeRole.AbmVls,
                    WeaponCascadeRole.AntiShipCoastal,
                    WeaponCascadeRole.AntiVehicleDrone
                }
            };

            var package = new ContractorPackage
            {
                PackageId = PackageId,
                DisplayName = "Vanguard Defence",
                Description = "Western precision modular 20ft ISO stealth air & missile defence line (Skyshield, ECCM-Hunter, ICBM-Hunter).",
                DefaultFaction = VanguardConfig.ParseConfiguredFaction(),
                AssetRootDirectory = assetRoot,
                BrandingDirectory = brandingDir,
                DefaultWtaProfile = wtaProfile
            };

            string skydartGlb = VanguardAssetResolver.ResolveGlbOrAlias(
                assetRoot,
                "skyshield_missile_skydart.glb",
                "skyshield_missile_eccm_hunter.glb");

            string skylanceGlb = VanguardAssetResolver.ResolveGlbOrAlias(
                assetRoot,
                "skyshield_missile_skylance.glb",
                "skyshield_missile_icbm_hunter.glb");

            string eccmGlb = VanguardAssetResolver.ResolveGlbOrAlias(
                assetRoot,
                "skyshield_missile_eccm_hunter.glb",
                "eccm_hunter_missile.glb");

            string icbmGlb = VanguardAssetResolver.ResolveGlbOrAlias(
                assetRoot,
                "skyshield_missile_icbm_hunter.glb",
                "abm_interceptor.glb");

            package.AddMissile(new CustomMissileSpec
            {
                MissileId = SkyDartSrSamMissileId,
                DisplayName = "SkyDart SR-SAM",
                GlbRelativePath = skydartGlb,
                FallbackGlbRelativePaths = new[] { "skyshield_missile_skydart.glb", "skyshield_missile_eccm_hunter.glb" },
                FallbackMeshArchetype = "eccm_hunter",
                BaseMissileCloneKey = "SAM_Radar2",
                DecalRelativePath = "branding/vanguard_skydart_decal.png",
                FlightProfile = MissileFlightProfile.Direct,
                ThermalHardening = 1.05f,
                IsDecoy = false,
                EnableFoldingFins = true,
                SupportsActiveToPassiveHoj = false,
                IsAbmInterceptor = false,
                MaxEngagementRangeKm = 14f,
                BlastYield = 30f,
                ModelYawOffsetDeg = 180f,
                CruiseSpeedMps = 750f,
                MaxSpeedMps = 800f,
                MaxMach = 2.35f,
                FinDeployDelaySeconds = 0.25f,
                RearmCostPerRound = 0.8f,
                IconRelativePath = "icons/icon_missile_skydart.png"
            });

            package.AddMissile(new CustomMissileSpec
            {
                MissileId = EccmHunterMissileId,
                DisplayName = "ECCM-Hunter \"SAME DAY DELIVERY\"",
                GlbRelativePath = eccmGlb,
                FallbackGlbRelativePaths = new[] { "eccm_hunter_missile.glb", "skyshield_missile_eccm_hunter.glb" },
                FallbackMeshArchetype = "eccm_hunter",
                BaseMissileCloneKey = "SAM_Radar2",
                DecalRelativePath = "branding/vanguard_eccm_side_decal.png",
                FlightProfile = MissileFlightProfile.Lofting,
                ThermalHardening = 1.10f,
                IsDecoy = false,
                EnableFoldingFins = true,
                SupportsActiveToPassiveHoj = true,
                IsAbmInterceptor = false,
                MaxEngagementRangeKm = 85f,
                BlastYield = 45f,
                ModelYawOffsetDeg = 180f,
                CruiseSpeedMps = 1120f,
                MaxSpeedMps = 1150f,
                MaxMach = 3.4f,
                LoftApexMeters = 6500f,
                FinDeployDelaySeconds = 0.30f,
                RearmCostPerRound = 5.0f,
                IconRelativePath = "icons/icon_missile_eccm_hunter.png",
                RequiresTargetEcmToTrack = true,
                WarningAlarmAudioRelativePath = "audio/eccm_hunter_rwr_loop.wav",
                WarningAlertAudioRelativePath = "audio/eccm_hunter_rwr_alert.wav",
                WarningAlarmVolume = 0.85f,
                WarningDetectionRangeMeters = 12000f
            });

            // Reserved cross-mod exo-atmospheric ABM interceptor (kept registered for MDS/cross-mod compatibility;
            // do NOT overwrite vgd_skyshield_vls's 4x ECCM-Hunter + 4x SkyLance cells with this missile).
            package.AddMissile(new CustomMissileSpec
            {
                MissileId = IcbmHunterMissileId,
                DisplayName = "ICBM-Hunter Exo-KKV",
                GlbRelativePath = icbmGlb,
                FallbackGlbRelativePaths = new[] { "abm_interceptor.glb", "skyshield_missile_icbm_hunter.glb" },
                FallbackMeshArchetype = "icbm_hunter",
                BaseMissileCloneKey = "SAM_Radar2",
                DecalRelativePath = "branding/vanguard_icbm_hunter_decal.png",
                FlightProfile = MissileFlightProfile.Ballistic,
                ThermalHardening = 1.20f,
                IsDecoy = false,
                EnableFoldingFins = true,
                SupportsActiveToPassiveHoj = false,
                IsAbmInterceptor = true,
                MaxEngagementRangeKm = 95f,
                BlastYield = 28f,
                ModelYawOffsetDeg = 180f,
                CruiseSpeedMps = 1450f,
                MaxSpeedMps = 1500f,
                MaxMach = 4.4f,
                FinDeployDelaySeconds = 0.25f,
                HighAltitudeInterceptMinMeters = 800f,
                DisarmTargetWarheadOnIntercept = true,
                RearmCostPerRound = 9.0f,
                IconRelativePath = "icons/icon_missile_icbm_hunter.png"
            });

            package.AddMissile(new CustomMissileSpec
            {
                MissileId = SkyLanceLrSamMissileId,
                DisplayName = "SkyLance LR-SAM (Long-Range AA)",
                GlbRelativePath = skylanceGlb,
                FallbackGlbRelativePaths = new[] { "skyshield_missile_skylance.glb", "skyshield_missile_icbm_hunter.glb" },
                FallbackMeshArchetype = "icbm_hunter",
                BaseMissileCloneKey = "SAM_Radar2",
                DecalRelativePath = "branding/vanguard_skylance_decal.png",
                FlightProfile = MissileFlightProfile.Lofting,
                ThermalHardening = 1.15f,
                IsDecoy = false,
                EnableFoldingFins = true,
                SupportsActiveToPassiveHoj = false,
                IsAbmInterceptor = false,
                MaxEngagementRangeKm = 42f,
                BlastYield = 55f,
                ModelYawOffsetDeg = 180f,
                CruiseSpeedMps = 1220f,
                MaxSpeedMps = 1260f,
                MaxMach = 3.8f,
                LoftApexMeters = 5500f,
                FinDeployDelaySeconds = 0.25f,
                RearmCostPerRound = 5.5f,
                IconRelativePath = "icons/icon_missile_skylance.png"
            });

            package.AddMissile(new CustomMissileSpec
            {
                MissileId = SeaWraithMissileId,
                DisplayName = "VGD SeaWraith",
                GlbRelativePath = "vgd_seawraith.glb",
                FallbackGlbRelativePaths = System.Array.Empty<string>(),
                FallbackMeshArchetype = "eccm_hunter",
                BaseMissileCloneKey = "AShM2",
                DecalRelativePath = "branding/vanguard_seawraith_decal.png",
                IconRelativePath = "icons/icon_missile_seawraith.png",
                FlightProfile = MissileFlightProfile.SeaSkimmingEvasive,
                IsAntiShip = true,
                ModelYawOffsetDeg = 180f,
                ThermalHardening = 1.15f,
                IsDecoy = false,
                EnableFoldingFins = true,
                FinDeployDelaySeconds = 0.30f,
                SupportsActiveToPassiveHoj = true,
                IsAbmInterceptor = false,
                TargetMach = 0.90f,
                CruiseSpeedMps = 306f,
                SeaSkimAltitudeMeters = 3.05f,
                StealthRcsOverride = 0.09f,
                SeekerEccmJamTolerance = 0.72f,
                HasActiveJammer = true,
                ActiveJammerIntensity = 0.22f,
                ActiveJammerRangeKm = 9.5f,
                ActiveJammerPulseIntervalSeconds = 1.35f,
                EnableTerminalEvasiveJink = true,
                EvasiveJinkStartRangeKm = 4.0f,
                EvasiveJinkAmplitudeMeters = 65f,
                EvasiveJinkPeriodSeconds = 3.0f,
                BoosterDropSeconds = 0f,
                MaxEngagementRangeKm = 50f,
                BlastYield = 125f,
                RearmCostPerRound = 6.5f,
                EnableAircraftMounting = true,
                AircraftMountCloneKeys = new[] { "AShM2", "AShM1", "CruiseMissile1", "AShM3" },
                AircraftMountMassKg = 416f,
                AircraftMountShortName = "SeaWraith"
            });

            package.AddMissile(new CustomMissileSpec
            {
                MissileId = WaspAtMissileId,
                DisplayName = "VGD Wasp-AT",
                GlbRelativePath = "vgd_wasp_at.glb",
                FallbackGlbRelativePaths = System.Array.Empty<string>(),
                FallbackMeshArchetype = "skydart",
                BaseMissileCloneKey = "AShM2",
                IconRelativePath = "icons/icon_missile_wasp_at.png",
                FlightProfile = MissileFlightProfile.LoiteringTopAttack,
                IsAntiVehicle = true,
                ModelYawOffsetDeg = 180f,
                ThermalHardening = 1.0f,
                IsDecoy = false,
                EnableFoldingFins = true,
                FinDeployDelaySeconds = 0.25f,
                SupportsActiveToPassiveHoj = false,
                IsAbmInterceptor = false,
                CruiseSpeedMps = 135f,
                MaxSpeedMps = 170f,
                TargetMach = 0.40f,
                MaxMach = 0.55f,
                LoiterCruiseAltitudeMeters = 220f,
                TopAttackDiveRangeMeters = 850f,
                StealthRcsOverride = 0.04f,
                MaxEngagementRangeKm = 18f,
                BlastYield = 13f,
                RearmCostPerRound = 1.5f,
                EnableAircraftMounting = false,
                MotorLoopAudioRelativePath = "audio/wasp_drone_motor_loop.wav",
                MotorLoopAudioVolume = 0.90f,
                MotorLoopAudioMinDistance = 28f,
                MotorLoopAudioMaxDistance = 1600f
            });

            float jamTol = VanguardConfig.AesaJamTolerance.Value;
            bool twoFcuBlink = VanguardConfig.EnableTwoFcuBlinking.Value;

            // 1. Skyshield Radar FCU (AESA + EO/IR + Passive ELINT)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = "vgd_skyshield_fcu",
                DisplayName = "Skyshield AESA FCU",
                Role = WeaponCascadeRole.FireControl,
                BaseDefinitionCloneKey = "radarStation1",
                GlbRelativePath = useContainers ? "skyshield_radar_fcu_container.glb" : "skyshield_radar_fcu.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_radar_fcu.glb", "skyshield_radar_fcu_container.glb" },
                DestroyedGlbRelativePath = "skyshield_radar_fcu_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_aesa_c2_decal.png",
                StowedRcs = 0.08f,
                DeployedRcs = 0.90f,
                RequiresFcuLink = false,
                MinEngagementRangeKm = 0f,
                MaxEngagementRangeKm = 48f,
                DeployDurationSeconds = 5.0f,
                SpinRadarDish = true,
                RadarSpinDegPerSec = 110f,
                FcuLinkRadiusKm = 18f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = true,
                    SupportedBands = new[]
                    {
                        SensorBand.XBand,
                        SensorBand.SBand,
                        SensorBand.EoIr,
                        SensorBand.PassiveElint
                    },
                    JamTolerance = jamTol,
                    EnableTwoFcuBlinking = twoFcuBlink,
                    EnableHomeOnJamTriangulation = true,
                    EoIrEmergencyRangeKm = 4.5f,
                    DetectionRangeKm = 48f,
                    BlinkOnSeconds = 6f,
                    BlinkOffSeconds = 4f,
                    AradEmconShutdownSeconds = 8f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 2,
                    CreditCost = 220f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 18f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 3400f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 2. Skyshield HEL (High-Energy Laser — Priority 1 in Western Cascade)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = "vgd_skyshield_hel",
                DisplayName = "Skyshield HEL Laser",
                Role = WeaponCascadeRole.LaserHel,
                BaseDefinitionCloneKey = "Emplacement1_23mm",
                GlbRelativePath = useContainers ? "skyshield_hel_container.glb" : "skyshield_hel.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_hel.glb", "skyshield_hel_container.glb" },
                DestroyedGlbRelativePath = "skyshield_hel_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_hel_decal.png",
                StowedRcs = 0.03f,
                DeployedRcs = 0.16f,
                RequiresFcuLink = true,
                MinEngagementRangeKm = 0.05f,
                MaxEngagementRangeKm = 4.2f,
                MagazineCapacity = 100,
                FireCooldownSeconds = 0.65f,
                DeployDurationSeconds = 5.0f,
                TurretTraverseDegPerSec = 135f,
                TurretElevationDegPerSec = 110f,
                MinElevationDeg = -10f,
                MaxElevationDeg = 88f,
                LaserPowerFactor = 1.15f,
                WeaponIconRelativePath = "icons/icon_weapon_hel_laser.png",
                FcuLinkRadiusKm = 18f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.EoIr, SensorBand.XBand },
                    JamTolerance = jamTol,
                    EoIrEmergencyRangeKm = 4.2f,
                    DetectionRangeKm = 8f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 2,
                    CreditCost = 300f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 20f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 3600f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 3. Skyshield 35mm AHEAD Gun CIWS
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = "vgd_skyshield_35mm",
                DisplayName = "Skyshield 35mm AHEAD",
                Role = WeaponCascadeRole.CloseInGun,
                CanEngageGroundVehicles = true,
                BaseDefinitionCloneKey = "Emplacement1_23mm",
                GlbRelativePath = useContainers ? "skyshield_gun_35mm_container.glb" : "skyshield_gun_35mm.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_gun_35mm.glb", "skyshield_gun_35mm_container.glb" },
                DestroyedGlbRelativePath = "skyshield_gun_35mm_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_ahead_35mm_decal.png",
                StowedRcs = 0.03f,
                DeployedRcs = 0.14f,
                RequiresFcuLink = false,
                MinEngagementRangeKm = 0.05f,
                MaxEngagementRangeKm = 3.6f,
                MagazineCapacity = 240,
                FireCooldownSeconds = 0.35f,
                DeployDurationSeconds = 5.0f,
                TurretTraverseDegPerSec = 120f,
                TurretElevationDegPerSec = 95f,
                MinElevationDeg = -10f,
                MaxElevationDeg = 85f,
                GunMuzzleVelocityMps = 1080f,
                GunDamagePerBurst = 36f,
                GunRearmCostPerRound = 0.12f,
                WeaponIconRelativePath = "icons/icon_ammo_35mm_ahead.png",
                GunProjectileGlbRelativePath = "skyshield_ammo_35mm_ahead.glb",
                GunProjectileVisualScale = 5.0f,
                GunFireAudioRelativePath = "audio/skyshield_gun_35mm_shot.wav",
                GunFireAudioVolume = 1.0f,
                GunFireAudioPitchMin = 0.96f,
                GunFireAudioPitchMax = 1.04f,
                GunFireAudioMinDistance = 32f,
                GunFireAudioMaxDistance = 2400f,
                GunBurstShotIntervalSeconds = 0.06f,
                FcuLinkRadiusKm = 18f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.XBand, SensorBand.EoIr },
                    JamTolerance = 0.78f,
                    EoIrEmergencyRangeKm = 3.5f,
                    DetectionRangeKm = 6f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 1,
                    CreditCost = 130f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 14f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 3300f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 4. Skyshield Short-Range SAM (Type C)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = "vgd_skyshield_sam_short",
                DisplayName = "Skyshield Short SAM (Type C)",
                Role = WeaponCascadeRole.ShortMedSam,
                BaseDefinitionCloneKey = "Emplacement1_MANPADS",
                GlbRelativePath = useContainers ? "skyshield_sam_short_c_container.glb" : "skyshield_sam_short_c.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_sam_short_c.glb", "skyshield_sam_short_c_container.glb" },
                DestroyedGlbRelativePath = "skyshield_sam_short_c_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_container_stencil.png",
                PrimaryMissileId = SkyDartSrSamMissileId,
                StowedRcs = 0.03f,
                DeployedRcs = 0.18f,
                RequiresFcuLink = true,
                MinEngagementRangeKm = 0.4f,
                MaxEngagementRangeKm = 14f,
                MagazineCapacity = 8,
                FireCooldownSeconds = 2.2f,
                DeployDurationSeconds = 5.0f,
                TurretTraverseDegPerSec = 85f,
                TurretElevationDegPerSec = 70f,
                MinElevationDeg = 35f,
                MaxElevationDeg = 75f,
                FcuLinkRadiusKm = 18f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.EoIr, SensorBand.XBand },
                    JamTolerance = jamTol,
                    EoIrEmergencyRangeKm = 4.5f,
                    DetectionRangeKm = 14f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 2,
                    CreditCost = 210f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 16f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 3200f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 5. Skyshield Long-Range SAM (Type A)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = "vgd_skyshield_sam_long",
                DisplayName = "Skyshield Long SAM (Type A)",
                Role = WeaponCascadeRole.LongRangeSam,
                BaseDefinitionCloneKey = "Emplacement1_MANPADS",
                GlbRelativePath = useContainers ? "skyshield_sam_long_a_container.glb" : "skyshield_sam_long_a.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_sam_long_a.glb", "skyshield_sam_long_a_container.glb" },
                DestroyedGlbRelativePath = "skyshield_sam_long_a_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_container_stencil.png",
                PrimaryMissileId = SkyLanceLrSamMissileId,
                StowedRcs = 0.03f,
                DeployedRcs = 0.22f,
                RequiresFcuLink = true,
                MinEngagementRangeKm = 1.5f,
                MaxEngagementRangeKm = 42f,
                MagazineCapacity = 8,
                FireCooldownSeconds = 3.5f,
                DeployDurationSeconds = 5.0f,
                TurretTraverseDegPerSec = 65f,
                TurretElevationDegPerSec = 55f,
                MinElevationDeg = 42f,
                MaxElevationDeg = 75f,
                FcuLinkRadiusKm = 18f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.XBand, SensorBand.EoIr },
                    JamTolerance = jamTol,
                    EoIrEmergencyRangeKm = 4.5f,
                    DetectionRangeKm = 42f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 3,
                    CreditCost = 450f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 22f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 3800f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 6. Skyshield 8-Cell VLS Container (4x ECCM-Hunter + 4x SkyLance LR-SAM; retains AbmVls cascade role)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = SkyshieldVlsUnitId,
                DisplayName = "Skyshield VLS (4x ECCM + 4x LR-SAM)",
                Role = WeaponCascadeRole.AbmVls,
                BaseDefinitionCloneKey = "Emplacement1_MANPADS",
                GlbRelativePath = "skyshield_vls_container.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_vls_container.glb" },
                DestroyedGlbRelativePath = "skyshield_vls_container_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_container_stencil.png",
                PrimaryMissileId = EccmHunterMissileId,
                SecondaryMissileId = SkyLanceLrSamMissileId,
                VerticalLaunch = true,
                StowedRcs = 0.03f,
                DeployedRcs = 0.24f,
                RequiresFcuLink = true,
                MinEngagementRangeKm = 2.0f,
                MaxEngagementRangeKm = 85f,
                MagazineCapacity = 8,
                PrimaryMagazineCapacity = 4,
                SecondaryMagazineCapacity = 4,
                FireCooldownSeconds = 3.0f,
                DeployDurationSeconds = 5.0f,
                FcuLinkRadiusKm = 22f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.XBand, SensorBand.SBand, SensorBand.PassiveElint },
                    JamTolerance = jamTol,
                    EnableHomeOnJamTriangulation = true,
                    EoIrEmergencyRangeKm = 3.5f,
                    DetectionRangeKm = 85f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 3,
                    CreditCost = 540f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 26f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 4200f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 7. Skyshield Leitstand (C2 Net — 45 km FCU Relay, Coverage-First WTA & CEC)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = LeitstandUnitId,
                DisplayName = "Skyshield Leitstand (C2 Net)",
                Role = WeaponCascadeRole.CommandControl,
                BaseDefinitionCloneKey = "radarStation1",
                GlbRelativePath = useContainers ? "skyshield_leitstand_container.glb" : "skyshield_leitstand.glb",
                FallbackGlbRelativePaths = new[] { "skyshield_leitstand.glb", "skyshield_leitstand_container.glb", "skyshield_radar_fcu_container.glb" },
                DestroyedGlbRelativePath = "skyshield_leitstand_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_aesa_c2_decal.png",
                IsCommandCenter = true,
                NetworkRelayRadiusKm = 45f,
                NetworkEccmBonus = 0.10f,
                NetworkDetectionBonusMultiplier = 1.12f,
                StowedRcs = 0.08f,
                DeployedRcs = 0.65f,
                RequiresFcuLink = false,
                MinEngagementRangeKm = 0f,
                MaxEngagementRangeKm = 45f,
                MagazineCapacity = 0,
                DeployDurationSeconds = 5.0f,
                SpinRadarDish = false,
                FcuLinkRadiusKm = 45f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.PassiveElint },
                    JamTolerance = 0.85f,
                    EnableHomeOnJamTriangulation = true,
                    EoIrEmergencyRangeKm = 0f,
                    DetectionRangeKm = 45f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 2,
                    CreditCost = 330f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 22f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 3500f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 8. SeaWraith 20ft ISO 6-Missile Elevating-Roof Container (Autonomous ASuW, Airdrop + Flatbed + Build)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = SeaWraithContainerUnitId,
                DisplayName = "SeaWraith Container",
                Role = WeaponCascadeRole.AntiShipCoastal,
                BaseDefinitionCloneKey = "Emplacement1_ATGM",
                GlbRelativePath = "vgd_seawraith_container.glb",
                DestroyedGlbRelativePath = "vgd_seawraith_container_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_seawraith_decal.png",
                PrimaryMissileId = SeaWraithMissileId,
                PrimaryMagazineCapacity = 6,
                MagazineCapacity = 6,
                FireCooldownSeconds = 3.2f,
                LaunchAlongSocketForward = true,
                VerticalLaunch = false,
                RequiresFcuLink = false,
                StowedRcs = 0.06f,
                DeployedRcs = 0.35f,
                MinEngagementRangeKm = 2.5f,
                MaxEngagementRangeKm = 52f,
                DeployDurationSeconds = 5.0f,
                ModelYawOffsetDeg = 0f,
                FcuLinkRadiusKm = 45f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.PassiveElint, SensorBand.EoIr },
                    JamTolerance = 0.88f,
                    EnableHomeOnJamTriangulation = true,
                    EoIrEmergencyRangeKm = 6.0f,
                    DetectionRangeKm = 40f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 2,
                    CreditCost = 460f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 22f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 4400f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 8B. Wasp Hive-80 20ft ISO 80-Drone Elevating-Roof Container (Anti-Vehicle Loitering Swarm)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = WaspHiveContainerUnitId,
                DisplayName = "Wasp Hive-80 Container",
                Role = WeaponCascadeRole.AntiVehicleDrone,
                CanEngageGroundVehicles = true,
                BaseDefinitionCloneKey = "Emplacement1_ATGM",
                GlbRelativePath = "vgd_wasp_hive_container.glb",
                DestroyedGlbRelativePath = "vgd_wasp_hive_container_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_wasp_decal.png",
                PrimaryMissileId = WaspAtMissileId,
                PrimaryMagazineCapacity = 24,
                MagazineCapacity = 24,
                FireCooldownSeconds = 2.0f,
                LaunchAlongSocketForward = true,
                VerticalLaunch = false,
                RequiresFcuLink = false,
                StowedRcs = 0.06f,
                DeployedRcs = 0.30f,
                MinEngagementRangeKm = 0.4f,
                MaxEngagementRangeKm = 18f,
                DeployDurationSeconds = 5.0f,
                ModelYawOffsetDeg = 0f,
                FcuLinkRadiusKm = 35f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = new[] { SensorBand.EoIr, SensorBand.PassiveElint },
                    JamTolerance = 0.82f,
                    EoIrEmergencyRangeKm = 4.5f,
                    DetectionRangeKm = 12f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 2,
                    CreditCost = 460f,
                    CapCategory = "Ground",
                    CapWeight = 2,
                    EngineerBuildSeconds = 20f,
                    SupportsTarantulaSlingKit = true,
                    SlingKitMassKg = 4200f,
                    SupportsAircraftAirdrop = true,
                    AirdropAltitudeMeters = 260f
                }
            });

            // 9. MAN HX3 8x8 ISO Container Carrier (Unarmed Logistics Flatbed Hauler)
            package.AddUnit(new ModularUnitSpec
            {
                UnitId = Hx3IsoCarrierUnitId,
                DisplayName = "HX3 8x8 ISO Flatbed",
                Role = WeaponCascadeRole.LogisticsHauler,
                BaseDefinitionCloneKey = "HLT-L",
                GlbRelativePath = "vgd_hx3_iso_carrier.glb",
                DestroyedGlbRelativePath = "vgd_hx3_iso_carrier_wreck.glb",
                DebrisKitGlbRelativePath = "skyshield_debris_kit.glb",
                DestroyAnimationDurationSeconds = 2.5f,
                DestructionDebrisCount = 6,
                FallbackMeshArchetype = "container",
                DecalRelativePath = "branding/vanguard_vehicle_side_decal.png",
                IsMobileVehicle = true,
                IsContainerHauler = true,
                WheelRadiusMeters = 0.670f,
                CargoDeckHeightMeters = 1.715f,
                CargoDeckOffsetZMeters = -2.100f,
                RequiresFcuLink = false,
                MagazineCapacity = 0,
                PrimaryMagazineCapacity = 0,
                StowedRcs = 0.25f,
                DeployedRcs = 0.35f,
                DeployDurationSeconds = 2.0f,
                ModelYawOffsetDeg = 0f,
                SensorProfile = new EccmSensorProfile
                {
                    IsFireControlUnit = false,
                    SupportedBands = System.Array.Empty<SensorBand>(),
                    JamTolerance = 0.5f,
                    EoIrEmergencyRangeKm = 0f,
                    DetectionRangeKm = 0f
                },
                Logistics = new DeploymentLogisticsSpec
                {
                    RequiredTechTier = 1,
                    CreditCost = 45f,
                    CapCategory = "Ground",
                    CapWeight = 1,
                    EngineerBuildSeconds = 12f,
                    SupportsTarantulaSlingKit = false,
                    SlingKitMassKg = 0f,
                    SupportsAircraftAirdrop = false,
                    AirdropAltitudeMeters = 0f
                }
            });

            var reportItems = new List<(string, string, string[])>();
            for (int i = 0; i < package.Units.Count; i++)
            {
                ModularUnitSpec u = package.Units[i];
                reportItems.Add((u.UnitId, u.GlbRelativePath, u.FallbackGlbRelativePaths));
                if (!string.IsNullOrWhiteSpace(u.GunProjectileGlbRelativePath))
                {
                    reportItems.Add(($"{u.UnitId}_ammo", u.GunProjectileGlbRelativePath, System.Array.Empty<string>()));
                }
            }

            for (int i = 0; i < package.Missiles.Count; i++)
            {
                CustomMissileSpec m = package.Missiles[i];
                reportItems.Add((m.MissileId, m.GlbRelativePath, m.FallbackGlbRelativePaths));
            }

            assetSummary = VanguardAssetResolver.BuildAssetReport(assetRoot, reportItems);
            return package;
        }

        internal static void ApplyLiveConfig(ContractorPackage? package)
        {
            if (package == null)
            {
                return;
            }

            if (CanMutateDefaultFaction())
            {
                package.DefaultFaction = VanguardConfig.ParseConfiguredFaction();
            }

            SmartEngagementProfile wta = package.DefaultWtaProfile;
            if (wta != null)
            {
                wta.PreferLaserBeforeMissiles = VanguardConfig.PreferLaserBeforeMissiles.Value;
                wta.LaserDwellSeconds = VanguardConfig.HelLaserDwellSeconds.Value;
                wta.AutoPopUpRangeKm = VanguardConfig.AutoPopUpRangeKm.Value;
            }

            bool useContainers = VanguardConfig.UseAnimatedIsoContainers.Value;
            float jamTol = VanguardConfig.AesaJamTolerance.Value;
            bool twoFcuBlink = VanguardConfig.EnableTwoFcuBlinking.Value;

            for (int i = 0; i < package.Units.Count; i++)
            {
                ModularUnitSpec unit = package.Units[i];
                if (unit == null)
                {
                    continue;
                }

                switch (unit.UnitId)
                {
                    case "vgd_skyshield_fcu":
                        unit.GlbRelativePath = useContainers ? "skyshield_radar_fcu_container.glb" : "skyshield_radar_fcu.glb";
                        break;
                    case "vgd_skyshield_hel":
                        unit.GlbRelativePath = useContainers ? "skyshield_hel_container.glb" : "skyshield_hel.glb";
                        break;
                    case "vgd_skyshield_35mm":
                        unit.GlbRelativePath = useContainers ? "skyshield_gun_35mm_container.glb" : "skyshield_gun_35mm.glb";
                        break;
                    case "vgd_skyshield_sam_short":
                        unit.GlbRelativePath = useContainers ? "skyshield_sam_short_c_container.glb" : "skyshield_sam_short_c.glb";
                        break;
                    case "vgd_skyshield_sam_long":
                        unit.GlbRelativePath = useContainers ? "skyshield_sam_long_a_container.glb" : "skyshield_sam_long_a.glb";
                        break;
                    case LeitstandUnitId:
                        unit.GlbRelativePath = useContainers ? "skyshield_leitstand_container.glb" : "skyshield_leitstand.glb";
                        break;
                }

                if (unit.SensorProfile == null)
                {
                    continue;
                }

                if (unit.UnitId == "vgd_skyshield_fcu")
                {
                    unit.SensorProfile.JamTolerance = jamTol;
                    unit.SensorProfile.EnableTwoFcuBlinking = twoFcuBlink;
                }
                else if (unit.UnitId != "vgd_skyshield_35mm" &&
                         unit.UnitId != LeitstandUnitId &&
                         unit.UnitId != SeaWraithContainerUnitId &&
                         unit.UnitId != WaspHiveContainerUnitId &&
                         unit.UnitId != Hx3IsoCarrierUnitId)
                {
                    unit.SensorProfile.JamTolerance = jamTol;
                }
            }

            RefreshLiveSensorProfiles(package);
        }

        private static bool CanMutateDefaultFaction()
        {
            if (MdsNetworkSync.HostHasMds)
            {
                return false;
            }

            try
            {
                PropertyInfo? pureClientProp = typeof(MdsNetworkSync).GetProperty(
                    "IsPureClient",
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (pureClientProp != null && pureClientProp.GetValue(null) is bool isPureClient && isPureClient)
                {
                    return false;
                }
            }
            catch
            {
                // HostHasMds already checked above.
            }

            return true;
        }

        private static void RefreshLiveSensorProfiles(ContractorPackage package)
        {
            try
            {
                Assembly mdsAsm = typeof(ContractorPackage).Assembly;
                Type? mgrType = mdsAsm.GetType("MDSFramework.Sensors.SensorNetworkManager");
                if (mgrType == null)
                {
                    return;
                }

                PropertyInfo? instanceProp = mgrType.GetProperty(
                    "Instance",
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                object? mgrInstance = instanceProp?.GetValue(null);
                if (mgrInstance == null)
                {
                    return;
                }

                PropertyInfo? allSitesProp = mgrType.GetProperty(
                    "AllSites",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (allSitesProp?.GetValue(mgrInstance) is System.Collections.IEnumerable sites)
                {
                    Type? siteType = mdsAsm.GetType("MDSFramework.Sensors.ModularSiteBehavior");
                    FieldInfo? pkgField = siteType?.GetField(
                        "Package",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    MethodInfo? applySensorMethod = siteType?.GetMethod(
                        "ApplyNativeSensorProfile",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    foreach (object? site in sites)
                    {
                        if (site == null)
                        {
                            continue;
                        }

                        object? sitePkg = pkgField?.GetValue(site);
                        if (ReferenceEquals(sitePkg, package))
                        {
                            applySensorMethod?.Invoke(site, null);
                        }
                    }
                }

                MethodInfo? refreshMethod = mgrType.GetMethod(
                    "RefreshLinksAndBlinking",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                refreshMethod?.Invoke(mgrInstance, new object[] { true });
            }
            catch
            {
                // Ignore when MDS runtime or scene sensor manager is inactive.
            }
        }
    }
}
