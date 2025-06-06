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

                return TryGetInstance(rootDir, versions);
            }

            static ICygwinSetupInstance? TryGetInstance(string installationPath, Interval<Version> versions)
            {
                string productPath = "Cygwin.bat";
                if (!File.Exists(Path.Combine(installationPath, productPath)))
                    return null;

                string mainModulePath = Path.Combine(installationPath, @"bin\cygwin1.dll");
                if (!File.Exists(mainModulePath))
                    return null;

                var versionInfo = FileVersionInfo.GetVersionInfo(mainModulePath);
                if (!Version.TryParse(versionInfo.ProductVersion, out var version))
                    return null;
                if (!versions.Contains(version))
                    return null;

                return new CygwinSetupInstance(version, installationPath, productPath);
            }
        }
    }
}
