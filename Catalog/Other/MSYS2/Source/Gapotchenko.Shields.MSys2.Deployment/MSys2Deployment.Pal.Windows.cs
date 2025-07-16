// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.FX.Text;
using Microsoft.Win32;

namespace Gapotchenko.Shields.MSys2.Deployment;

partial class MSys2Deployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<IMSys2SetupInstance> EnumerateSetupInstances(Interval<Version> versions, MSys2DiscoveryOptions options)
            {
                using var hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
                return EnumerateSetupInstancesFromUserHive(hkcu, versions, options);
            }

            static IEnumerable<IMSys2SetupInstance> EnumerateSetupInstancesFromUserHive(
                RegistryKey userHive,
                Interval<Version> versions,
                MSys2DiscoveryOptions options)
            {
                using var uninstallKey = userHive.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
                if (uninstallKey is not null)
                {
                    // 2025-06-05: this is the only way to precisely locate an MSYS2 setup,
                    // no other information is available. No HKLM keys, no upgrade codes, nothing
                    // except a tiny ARP record (ARP = "Add/Remove Programs") at the user registry hive.
                    foreach (string keyName in uninstallKey.GetSubKeyNames())
                    {
                        // Act only on keys having "{<guid>}" format, only MSYS2 and a few other things have them.
                        if (keyName.StartsWith('{') && keyName.EndsWith('}') && Guid.TryParse(keyName, out _))
                        {
                            using var key = uninstallKey.OpenSubKey(keyName);
                            if (key is not null)
                            {
                                var instance = TryGetInstance(key, versions, options);
                                if (instance is not null)
                                    yield return instance;
                            }
                        }
                    }
                }
            }

            static IMSys2SetupInstance? TryGetInstance(
                RegistryKey key,
                Interval<Version> versions,
                MSys2DiscoveryOptions options)
            {
                if (key.GetValue("DisplayName") is not string displayName)
                    return null;
                if (!displayName.Equals("MSYS2", StringComparison.Ordinal))
                    return null;

                if (key.GetValue("DisplayVersion") is not string displayVersion)
                    return null;
                if (MSys2SetupInstance.TryParseVersion(displayVersion) is not { } version)
                    return null; // cannot parse the version
                if (!versions.Contains(version))
                    return null; // not asked for this version

                string? installLocation = key.GetValue("InstallLocation") as string;
                if (!Directory.Exists(installLocation))
                    return null;

                return MSys2SetupInstance.TryCreate(installLocation, version, options);
            }
        }
    }
}
