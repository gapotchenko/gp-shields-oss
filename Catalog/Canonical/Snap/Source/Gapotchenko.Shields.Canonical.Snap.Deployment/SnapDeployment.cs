// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

/// <summary>
/// Provides deployment discovery services for Canonical Snap technology.
/// </summary>
public static partial class SnapDeployment
{
    /// <summary>
    /// Enumerates setup instances of Canonical Snap.
    /// </summary>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of Canonical Snap setup instances.</returns>
    public static IEnumerable<ISnapSetupInstance> EnumerateSetupInstances(SnapDiscoveryOptions options = default)
    {
        var query = EnumerateSetupInstancesCore();

        if ((options & SnapDiscoveryOptions.NoSort) == 0)
        {
            // Hint: setup instances can be sorted here.
        }

        return query;
    }

    static IEnumerable<ISnapSetupInstance> EnumerateSetupInstancesCore()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Pal.Linux.EnumerateSetupInstances();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return Pal.MacOS.EnumerateSetupInstances();
        else
            return [];
    }
}
