// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Deployment;

partial class BrewDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("macos")]
#endif
        public static class MacOS
        {
            public static IEnumerable<BrewSetupDescriptor> EnumerateSetupDescriptors()
            {
                string probingPath = "/opt/homebrew";
                if (Directory.Exists(probingPath))
                    yield return new(probingPath);
            }
        }
    }
}
