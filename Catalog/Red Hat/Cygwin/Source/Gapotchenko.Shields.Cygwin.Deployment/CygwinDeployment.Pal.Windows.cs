// Gapotchenko.Shields.Cygwin
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;
using Microsoft.Win32;

namespace Gapotchenko.Shields.Cygwin.Deployment;

partial class CygwinDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<ICygwinSetupInstance> EnumerateSetupInstances(Interval<Version> versions)
            {
                var instance = TryGetInstanceFromRegistry(versions);
                if (instance is not null)
                    yield return instance;
            }

            static ICygwinSetupInstance? TryGetInstanceFromRegistry(Interval<Version> versions)
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var key = hklm.OpenSubKey(@"SOFTWARE\Cygwin\setup");
                if (key is null)
                    return null;

                string? rootDir = key.GetValue("rootdir") as string;
                if (!Directory.Exists(rootDir))
                    return null;

                return CygwinSetupInstance.TryCreate(rootDir, versions);
            }
        }
    }
}
