// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;
using Microsoft.Win32;

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

partial class WslDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<IWslSetupInstance> EnumerateSetupInstances(Interval<Version> versions)
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss\MSI");
                if (key is null)
                    yield break;

                if (TryGetInstance(key, versions) is { } instance)
                    yield return instance;
            }

            static IWslSetupInstance? TryGetInstance(RegistryKey key, Interval<Version> versions)
            {
                if (key.GetValue("Version") is not string versionString)
                    return null;
                if (!Version.TryParse(versionString, out var version))
                    return null;
                if (!versions.Contains(version))
                    return null;

                string? installLocation = key.GetValue("InstallLocation") as string;
                if (!Directory.Exists(installLocation))
                    return null;

                return WslSetupInstance.TryCreate(installLocation, version);
            }
        }
    }
}
