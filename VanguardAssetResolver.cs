using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VanguardDefence
{
    internal static class VanguardAssetResolver
    {
        internal static string ResolveAssetRoot(string pluginDirectory)
        {
            string custom = VanguardConfig.CustomAssetRootOverride.Value;
            if (!string.IsNullOrWhiteSpace(custom) && Directory.Exists(custom))
            {
                return custom;
            }

            string bundledAssets = Path.Combine(pluginDirectory, "Assets");
            if (Directory.Exists(bundledAssets))
            {
                return bundledAssets;
            }

            string? parentDir = Directory.GetParent(pluginDirectory)?.FullName;
            if (!string.IsNullOrEmpty(parentDir))
            {
                string devModels = Path.Combine(parentDir, "Models_Skyshield");
                if (Directory.Exists(devModels))
                {
                    return devModels;
                }
            }

            return pluginDirectory;
        }

        internal static string ResolveGlbOrAlias(string assetRoot, string preferredRelative, params string[] aliases)
        {
            if (File.Exists(Path.Combine(assetRoot, preferredRelative)))
            {
                return preferredRelative;
            }

            for (int i = 0; i < aliases.Length; i++)
            {
                if (File.Exists(Path.Combine(assetRoot, aliases[i])))
                {
                    return aliases[i];
                }
            }

            return preferredRelative;
        }

        internal static string BuildAssetReport(
            string assetRoot,
            IEnumerable<(string Id, string Primary, string[] Fallbacks)> entries)
        {
            var sb = new StringBuilder();
            sb.Append($"Asset root: '{assetRoot}' -> ");
            int found = 0;
            int total = 0;
            foreach ((string id, string primary, string[] fallbacks) in entries)
            {
                total++;
                bool exists = File.Exists(Path.Combine(assetRoot, primary));
                string chosen = primary;
                if (!exists && fallbacks != null)
                {
                    for (int i = 0; i < fallbacks.Length; i++)
                    {
                        if (File.Exists(Path.Combine(assetRoot, fallbacks[i])))
                        {
                            exists = true;
                            chosen = fallbacks[i];
                            break;
                        }
                    }
                }

                if (exists)
                {
                    found++;
                }

                sb.Append($"[{id}: {(exists ? chosen : "procedural-fallback")}] ");
            }

            sb.Append($"({found}/{total} GLBs on disk)");
            return sb.ToString();
        }
    }
}
