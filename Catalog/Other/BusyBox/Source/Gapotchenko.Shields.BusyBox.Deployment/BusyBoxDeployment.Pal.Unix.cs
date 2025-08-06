// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.BusyBox.Deployment;

partial class BusyBoxDeployment
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
            public static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors()
            {
                foreach (string prefix in new string[] { "/usr/local", "/usr", "/" })
                {
                    string path = Path.Combine(prefix, "bin/busybox");
                    if (File.Exists(path))
                        yield return new BusyBoxSetupDescriptor(GetRealPath(path));
                }
            }

            public static bool TryResolveInstallationPath(
                in BusyBoxSetupDescriptor descriptor,
                [MaybeNullWhen(false)] out string installationPath,
                [MaybeNullWhen(false)] out string productPath)
            {
                const string productFileName = "busybox";

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
