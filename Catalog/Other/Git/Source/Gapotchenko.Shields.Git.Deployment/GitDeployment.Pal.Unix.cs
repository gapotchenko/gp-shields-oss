// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Git.Deployment;

partial class GitDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("freebsd")]
#endif
        public static class Unix
        {
            public static IEnumerable<GitSetupDescriptor> EnumerateSetupDescriptors()
            {
                string path = "/usr/bin/git";
                if (File.Exists(path))
                    yield return new GitSetupDescriptor(GetRealPath(path));
            }

            public static bool TryDetermineInstallationPath(
                in GitSetupDescriptor descriptor,
                [MaybeNullWhen(false)] out string installationPath,
                [MaybeNullWhen(false)] out string productPath)
            {
                if (descriptor.ProductPath is "/usr/bin/git")
                {
                    installationPath = "/usr";
                    productPath = "bin/git";
                    return true;
                }
                else
                {
                    installationPath = default;
                    productPath = default;
                    return false;
                }
            }
        }
    }
}
