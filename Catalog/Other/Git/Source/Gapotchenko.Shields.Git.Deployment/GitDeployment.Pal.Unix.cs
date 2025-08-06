// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

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

            public static bool TryResolveInstallationPath(
                in GitSetupDescriptor descriptor,
                [MaybeNullWhen(false)] out string installationPath,
                [MaybeNullWhen(false)] out string productPath)
            {
                const string productFileName = "git";

                switch (descriptor.ProductPath)
                {
                    // Embedded into the system.
                    case $"/bin/{productFileName}":
                        installationPath = "/";
                        productPath = $"bin/{productFileName}";
                        return true;

                    // Preinstalled in the system.
                    case $"/usr/bin/{productFileName}":
                        installationPath = "/usr";
                        productPath = $"bin/{productFileName}";
                        return true;

                    // Installed on the system.
                    case $"/usr/local/bin/{productFileName}":
                        installationPath = "/usr/local";
                        productPath = $"bin/{productFileName}";
                        return true;

                    default:
                        installationPath = default;
                        productPath = default;
                        return false;
                }
            }
        }
    }
}
