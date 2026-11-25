using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BepInEx;
using MDSFramework.Api;

namespace VanguardDefence
{
    [BepInPlugin(VanguardPluginInfo.Guid, VanguardPluginInfo.Name, VanguardPluginInfo.Version)]
    [BepInDependency(VanguardPluginInfo.MdsFrameworkGuid, BepInDependency.DependencyFlags.HardDependency)]
    [SuppressMessage("Usage", "CA2243:Attribute string literals should parse correctly", Justification = "BepInEx plugin GUID uses reverse-DNS format.")]
    public sealed class VanguardPlugin : BaseUnityPlugin
    {
        internal static VanguardPlugin? Instance { get; private set; }
        internal ContractorPackage? RegisteredPackage { get; private set; }

        private void Awake()
        {
            Instance = this;
            VanguardConfig.Initialize(Config);

            if (!VanguardConfig.ModEnabled.Value)
            {
                Logger.LogInfo($"{VanguardPluginInfo.Name} {VanguardPluginInfo.Version} is disabled in config.");
                return;
            }

            string pluginDir = Path.GetDirectoryName(Info.Location) ?? AppContext.BaseDirectory;
            ContractorPackage package = VanguardPackageBuilder.Build(pluginDir, out string assetSummary);
            ContractorRegistry.RegisterPackage(package);
            RegisteredPackage = package;

            Logger.LogInfo(
                $"{VanguardPluginInfo.Name} {VanguardPluginInfo.Version} registered '{package.PackageId}' " +
                $"(Faction={package.DefaultFaction}). {assetSummary}");
        }

        private void OnDestroy()
        {
            if (ReferenceEquals(Instance, this))
            {
                Instance = null;
            }
        }
    }
}
