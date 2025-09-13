// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

/// <summary>
/// Provides deployment discovery services for Microsoft Windows Subsystem for Linux (WSL).
/// </summary>
public static partial class WslDeployment
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
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(versions);
        if ((options & WslDiscoveryOptions.NoSort) == 0)
            query = query.OrderByDescending(x => x.Version);
        return query;
    }

    static IEnumerable<IWslSetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Pal.Windows.EnumerateSetupInstances(versions);
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Pal.Linux.EnumerateSetupInstances(versions);
        else
            return [];
    }
}

/*

TODO:

IWslSetupInstance:
   IWslSetupInstanceDistributions Distributions { get; }

IWslSetupInstanceDistributions
   IEnumerable<IWslDistributionSetupInstance> EnumerateInstalled();

IWslDistributionSetupInstance
   string Name { get; }

*/
