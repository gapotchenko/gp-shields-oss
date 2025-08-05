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

            public static bool TryDetermineInstallationPath(
                in BusyBoxSetupDescriptor descriptor,
                [MaybeNullWhen(false)] out string installationPath,
                [MaybeNullWhen(false)] out string productPath)
            {
                switch (descriptor.ProductPath)
                {
                    case "/bin/busybox":
                        installationPath = "/";
                        productPath = "bin/busybox";
                        return true;
                    case "/usr/bin/busybox":
                        installationPath = "/usr";
                        productPath = "bin/busybox";
                        return true;
                    case "/usr/local/bin/busybox":
                        installationPath = "/usr/local";
                        productPath = "bin/busybox";
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
