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
        [SupportedOSPlatform("linux")]
#endif
        public static class Linux
        {
            public static IEnumerable<BrewSetupDescriptor> EnumerateSetupDescriptors()
            {
                string probingPath = "/home/linuxbrew/.linuxbrew";
                if (Directory.Exists(probingPath))
                    yield return new(probingPath);
            }
        }
    }
}
