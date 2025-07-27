// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;
using Microsoft.Win32;

namespace Gapotchenko.Shields.Git.Deployment;

partial class GitDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<GitSetupDescriptor> EnumerateSetupDescriptors(Interval<Version> versions)
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var key = hklm.OpenSubKey(@"SOFTWARE\GitForWindows");
                if (key is not null && TryGetDescriptor(key, versions) is { } descriptor)
                    yield return descriptor;
            }

            static GitSetupDescriptor? TryGetDescriptor(RegistryKey key, Interval<Version> versions)
            {
                if (key.GetValue("CurrentVersion") is not string versionString)
                    return null;
                if (!Version.TryParse(versionString, out var version))
                    return null;
                if (!versions.Contains(version))
                    return null;

                string? installationPath = key.GetValue("InstallPath") as string;
                if (!Directory.Exists(installationPath))
                    return null;

                string? libExecPath = key.GetValue("LibexecPath") as string;
                if (!Directory.Exists(libExecPath))
                    return null;

                string productPath = @"cmd\git.exe";
                if (!File.Exists(Path.Combine(installationPath, productPath)))
                    return null;

                return
                    new GitSetupDescriptor(productPath)
                    {
                        Version = version,
                        InstallationPath = installationPath,
                        LibExecPath = libExecPath
                    };
            }

            public static bool TryDetermineInstallationPath(
                in GitSetupDescriptor descriptor,
                [MaybeNullWhen(false)] out string installationPath,
                [MaybeNullWhen(false)] out string productPath)
            {
                string path = descriptor.ProductPath;
                if (path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    string? directory = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        string container = Path.GetFileName(directory);
                        if (container is "cmd" or "bin")
                        {
                            directory = Path.GetDirectoryName(directory);
                            if (!string.IsNullOrEmpty(directory))
                            {
                                if (File.Exists(Path.Combine(directory, "git-cmd.exe")))
                                {
                                    installationPath = directory;
                                    productPath = Path.Combine(container, Path.GetFileName(path));
                                    return true;
                                }
                            }
                        }
                    }
                }

                installationPath = default;
                productPath = default;
                return false;
            }
        }
    }
}
