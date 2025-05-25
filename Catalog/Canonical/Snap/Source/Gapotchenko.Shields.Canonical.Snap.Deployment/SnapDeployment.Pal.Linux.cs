// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shards.Diagnostics.CommandLine.Shell;

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

partial class SnapDeployment
{
    static partial class Pal
    {
#if NET
        [SupportedOSPlatform("linux")]
#endif
        public static class Linux
        {
            public static IEnumerable<ISnapSetupInstance> EnumerateSetupInstances()
            {
                string installationPath = "/snap";
                if (Directory.Exists(installationPath))
                {
                    var attributes = SnapSetupInstanceAttributes.None;

                    string? productPath = "/usr/bin/snap";
                    if (!File.Exists(productPath))
                    {
                        productPath = CommandLineShellOperations.Where("snap").FirstOrDefault();
                        if (productPath != null)
                            attributes |= SnapSetupInstanceAttributes.Path;
                    }

                    if (productPath != null)
                        yield return new SnapSetupInstance(installationPath, productPath, attributes);
                }
            }
        }
    }
}
