// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Diagnostics;
using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;
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
                return
                    EnumerateRegistrySetupInstances(versions, options)
                    .Concat(EnumerateEnvironmentSetupInstances(versions, options));
            }

            static IEnumerable<IMSys2SetupInstance> EnumerateRegistrySetupInstances(Interval<Version> versions, MSys2DiscoveryOptions options)
            {
                using var hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
                return EnumerateFromUserHive(hkcu, versions, options);

                static IEnumerable<IMSys2SetupInstance> EnumerateFromUserHive(
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

                    return MSys2SetupInstance.TryCreate(installLocation, version, MSys2SetupInstanceAttributes.None, options);
                }
            }

            static IEnumerable<IMSys2SetupInstance> EnumerateEnvironmentSetupInstances(Interval<Version> versions, MSys2DiscoveryOptions options)
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_WORKFLOW")))
                {
                    // MSYS2 can be installed via GitHub action 'msys2/setup-msys2@v2'.
                    // https://www.msys2.org/docs/ci/

                    var query =
                        CommandShell.Which("msys2.cmd")
                        .Take(1)
                        .Select(msys2FilePath => TryGetInstanceFromCommandFile(msys2FilePath, versions, options));

                    foreach (var instance in query)
                    {
                        if (instance != null)
                            yield return instance;
                    }

                    static IMSys2SetupInstance? TryGetInstanceFromCommandFile(string msys2FilePath, Interval<Version> versions, MSys2DiscoveryOptions options)
                    {
                        string? line =
                            File.ReadLines(msys2FilePath)
                            .Select(line => line.Trim())
                            .Where(line => line is not [] && !line.StartsWith("REM ", StringComparison.OrdinalIgnoreCase))
                            .LastOrDefault();

                        if (line == null)
                            return null;

                        // Line samples:
                        //  - D:\a\_temp\msys64\usr\bin\bash.exe -leo pipefail %*

                        int j = line.IndexOf(@"\usr\bin\bash.exe ", FileSystem.PathComparison);
                        if (j == -1)
                            return null;

                        string installationPath = line[..j].TrimStart('"');

#if !NETCOREAPP2_1_OR_GREATER
                        try
#endif
                        {
                            if (!Path.IsPathRooted(installationPath))
                            {
                                // Most probably the line was parsed incorrectly.
                                return null;
                            }
                        }
#if !NETCOREAPP2_1_OR_GREATER
                        catch (ArgumentException)
                        {
                            // .NET Framework and .NET Core versions older than 2.1:
                            // the path contains one or more invalid characters.
                            return null;
                        }
#endif

                        var instance = MSys2SetupInstance.TryCreate(installationPath, null, MSys2SetupInstanceAttributes.Environment, options);
                        if (instance == null)
                            return null;

                        if (!versions.Contains(instance.Version))
                            return null;

                        return instance;
                    }
                }
            }
        }
    }
}
