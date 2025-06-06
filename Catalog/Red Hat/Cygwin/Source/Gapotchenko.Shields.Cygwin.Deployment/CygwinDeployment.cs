// Gapotchenko.Shields.Cygwin
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Cygwin.Deployment;

/// <summary>
/// Provides deployment discovery services for Cygwin.
/// </summary>
public static partial class CygwinDeployment
{
    /// <inheritdoc cref="EnumerateSetupInstances(ValueInterval{Version}, CygwinDiscoveryOptions)"/>
    public static IEnumerable<ICygwinSetupInstance> EnumerateSetupInstances(CygwinDiscoveryOptions options = default) =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>(), options);

    /// <summary>
    /// Enumerates setup instances of Cygwin.
    /// By default, the instances are sorted by the product version and the newest versions come first.
    /// </summary>
    /// <param name="versions">The interval of Cygwin versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of Cygwin.</returns>
    public static IEnumerable<ICygwinSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        CygwinDiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(versions);

        if ((options & CygwinDiscoveryOptions.NoSort) == 0)
            query = query.OrderByDescending(x => x.Version);

        return query;
    }

    static IEnumerable<ICygwinSetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Pal.Windows.EnumerateSetupInstances(versions);
        else
            return [];
    }
}
