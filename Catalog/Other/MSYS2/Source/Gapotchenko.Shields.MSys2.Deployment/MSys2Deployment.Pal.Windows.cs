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
            public static IEnumerable<MSys2SetupDescriptor> EnumerateSetupDescriptors(Interval<Version> versions, MSys2DiscoveryOptions options)
            {
                var query = EnumerateRegistry(versions);

                if ((options & MSys2DiscoveryOptions.NoEnvironment) == 0)
                    query = query.Concat(EnumerableEx.Lazy(() => EnumerateEnvironment(options)));

                return query;
            }

            static IEnumerable<MSys2SetupDescriptor> EnumerateRegistry(Interval<Version> versions)
            {
                using var hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
                return EnumerateFromUserHive(hkcu, versions);

                static IEnumerable<MSys2SetupDescriptor> EnumerateFromUserHive(RegistryKey userHive, Interval<Version> versions)
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
                                    if (TryGetDescriptor(key, versions) is { } descriptor)
                                        yield return descriptor;
                                }
                            }
                        }
                    }
                }

                static MSys2SetupDescriptor? TryGetDescriptor(RegistryKey key, Interval<Version> versions)
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

                    return new MSys2SetupDescriptor(installLocation) { Version = version };
                }
            }

            #region Environment

            static IEnumerable<MSys2SetupDescriptor> EnumerateEnvironment(MSys2DiscoveryOptions options)
            {
                if (Environment.GetEnvironmentVariable("GHCUP_MSYS2") is { } installationPath and not [])
                {
                    // The value points to an MSYS2 instance provided by 'GHCup' utility which is related to GNU Haskell compiler.
                    // For example, this is a way MSYS2 is preinstalled on GitHub runners as of July 2025.

                    yield return new MSys2SetupDescriptor(installationPath) { Attributes = MSys2SetupInstanceAttributes.Environment };
                }

                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_WORKFLOW")))
                {
                    foreach (var descriptor in EnumerateGitHubAction(options))
                        yield return descriptor;
                }
            }

            static IEnumerable<MSys2SetupDescriptor> EnumerateGitHubAction(MSys2DiscoveryOptions options)
            {
                // MSYS2 can be installed via GitHub action 'msys2/setup-msys2@v2'.
                // https://www.msys2.org/docs/ci/

                if ((options & MSys2DiscoveryOptions.NoPath) == 0)
                {
                    var query =
                        CommandShell.Where("msys2.cmd")
                        .Take(1)
                        .Select(msys2FilePath => TryGetDescriptorFromCommandFile(msys2FilePath, options));

                    foreach (var descriptor in query)
                    {
                        if (descriptor is { } value)
                            yield return value;
                    }

                    static MSys2SetupDescriptor? TryGetDescriptorFromCommandFile(string msys2FilePath, MSys2DiscoveryOptions options)
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
                                // If we are here, it means that most probably the line was parsed incorrectly.
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

                        return new MSys2SetupDescriptor(installationPath) { Attributes = MSys2SetupInstanceAttributes.Environment };
                    }
                }
            }

            #endregion
        }
    }
}
