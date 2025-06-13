// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Provides deployment discovery services for MSYS2 software distribution and building platform.
/// </summary>
public static partial class MSys2Deployment
{
    /// <inheritdoc cref="EnumerateSetupInstances(ValueInterval{Version}, MSys2DiscoveryOptions)"/>
    public static IEnumerable<IMSys2SetupInstance> EnumerateSetupInstances(MSys2DiscoveryOptions options = default) =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>(), options);

    /// <summary>
    /// Enumerates setup instances of MSYS2.
    /// By default, the instances are sorted by the product version and the newest versions come first.
    /// </summary>
    /// <param name="versions">The interval of MSYS2 versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of MSYS2.</returns>
    public static IEnumerable<IMSys2SetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        MSys2DiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(versions, options);

        if ((options & MSys2DiscoveryOptions.NoSort) == 0)
            query = query.OrderByDescending(x => x.Version);

        return query;
    }

    static IEnumerable<IMSys2SetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions, MSys2DiscoveryOptions options)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Pal.Windows.EnumerateSetupInstances(versions, options);
        else
            return [];
    }
}
