// Gapotchenko.Shields.Canonical.Snap.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

partial class SnapDeployment
{
    static partial class Pal
    {
#if NET
        [SupportedOSPlatform("macos")]
#endif
        public static class MacOS
        {
            public static IEnumerable<ISnapSetupInstance> EnumerateSetupInstances()
            {
                // Snap can be installed on macOS but cannot be realistically used, yet.
                return [];
            }
        }
    }
}
