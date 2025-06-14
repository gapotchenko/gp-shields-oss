// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;
using Microsoft.Win32;

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

/// <summary>
/// Provides deployment discovery services for Microsoft Windows Subsystem for Linux (WSL).
/// </summary>
public static class WslDeployment
{
    /// <inheritdoc cref="EnumerateSetupInstances(ValueInterval{Version}, WslDiscoveryOptions)"/>
    public static IEnumerable<IWslSetupInstance> EnumerateSetupInstances(WslDiscoveryOptions options = default) =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>(), options);

    /// <summary>
    /// Enumerates setup instances of Microsoft WSL.
    /// By default, the instances are sorted by the product version and the newest versions come first.
    /// </summary>
    /// <param name="versions">The interval of WSL versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of WSL.</returns>
    public static IEnumerable<IWslSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        WslDiscoveryOptions options = default)
    {
        if (versions.IsEmpty || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return [];

        var query = EnumerateSetupInstancesCore(versions);
        if ((options & WslDiscoveryOptions.NoSort) == 0)
            query = query.OrderByDescending(x => x.Version);
        return query;
    }

#if NET
    [SupportedOSPlatform("windows")]
#endif
    static IEnumerable<IWslSetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions)
    {
        using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss\MSI");
        if (key is null)
            yield break;

        if (TryGetInstance(key, versions) is { } instance)
            yield return instance;
    }

#if NET
    [SupportedOSPlatform("windows")]
#endif
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

        string productPath = "wsl.exe";
        if (!File.Exists(Path.Combine(installLocation, productPath)))
            return null;

        return new WslSetupInstance(installLocation, version, productPath);
    }
}
