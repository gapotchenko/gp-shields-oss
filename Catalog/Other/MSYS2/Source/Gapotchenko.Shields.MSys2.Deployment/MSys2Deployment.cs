// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;
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
        {
            query = query.Memoize();
            if (query.CountIsAtLeast(2))
            {
                // Sort only if there are two or more instances.
                // This precaution is necessary to avoid potentially expensive version
                // retrieving operations when they are not strictly needed.

                var prioritizedAttributeMask = MSys2SetupInstanceAttributes.None;
                if ((options & MSys2DiscoveryOptions.EnvironmentInvariant) == 0)
                {
                    // Prefer setup instances deducted from the environment
                    // because it gives a configuration flexibility to a user.
                    prioritizedAttributeMask |= MSys2SetupInstanceAttributes.Environment;
                }

                query = query
                    .OrderByDescending(instance => (instance.Attributes & prioritizedAttributeMask) != 0)
                    .ThenByDescending(instance => instance.Version);
            }
        }

        return query;
    }

    static IEnumerable<IMSys2SetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions, MSys2DiscoveryOptions options)
    {
        return
            EnumerateSetupDescriptors(versions, options)
            .GroupBy(x => x.InstallationPath, FileSystem.PathEquivalenceComparer)
            .Select(g => MSys2SetupInstance.TryCreate(g.Key, g, versions, options))
            .Where(x => x != null)!;
    }

    static IEnumerable<MSys2SetupDescriptor> EnumerateSetupDescriptors(Interval<Version> versions, MSys2DiscoveryOptions options)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Pal.Windows.EnumerateSetupDescriptors(versions, options);
        else
            return [];
    }
}
