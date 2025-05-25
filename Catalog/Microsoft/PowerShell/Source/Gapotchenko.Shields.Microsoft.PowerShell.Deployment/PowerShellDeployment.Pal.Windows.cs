// Gapotchenko.Shields.Microsoft.PowerShell
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.FX.Math.Intervals;
using Microsoft.Win32;

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

partial class PowerShellDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<IPowerShellSetupInstance> EnumerateSetupInstances(
                Interval<Version> versions,
                PowerShellDiscoveryOptions options)
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\PowerShell\1");
                if (key is not null)
                {
                    var instance = TryGetInstance(key);
                    if (instance is not null)
                        yield return instance;
                }
            }

            static IPowerShellSetupInstance? TryGetInstance(RegistryKey key)
            {
                using var engineKey = key.OpenSubKey("PowerShellEngine");
                if (engineKey is null)
                    return null;

                if (engineKey.GetValue("ApplicationBase") is not string applicationBase)
                    return null;

                using var shellIdKey = key.OpenSubKey(@"ShellIds\Microsoft.PowerShell");
                if (shellIdKey is null)
                    return null;

                if (shellIdKey.GetValue("Path") is not string productPath)
                    return null;

                return new PowerShellSetupInstance(applicationBase, productPath);
            }
        }
    }
}
