// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Diagnostics;
using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.BusyBox.Deployment;

/// <summary>
/// Provides deployment discovery services for BusyBox toolkit.
/// </summary>
public static partial class BusyBoxDeployment
{
    /// <inheritdoc cref="EnumerateSetupInstances(ValueInterval{Version}, BusyBoxDiscoveryOptions)"/>
    public static IEnumerable<IBusyBoxSetupInstance> EnumerateSetupInstances(BusyBoxDiscoveryOptions options = default) =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>(), options);

    /// <summary>
    /// Enumerates setup instances of BusyBox.
    /// </summary>
    /// <remarks>
    /// By default, BusyBox setup instances are sorted by the product version and edition.
    /// The newest and most suitable versions come first.
    /// </remarks>
    /// <param name="versions">The interval of BusyBox versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of BusyBox.</returns>
    public static IEnumerable<IBusyBoxSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        BusyBoxDiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(BusyBoxVersion.NaturalizeInterval(versions), options);

        query = query.Memoize();
        if (query.CountIsAtLeast(2))
        {
            // Sort only if there are two or more instances.
            // This precaution is necessary to avoid potentially expensive
            // data retrieving operations when they are not strictly needed.

            var prioritizedAttributeMask = BusyBoxSetupInstanceAttributes.None;
            if ((options & BusyBoxDiscoveryOptions.EnvironmentInvariant) == 0)
            {
                // Prefer setup instances deducted from the environment
                // because it gives a configuration flexibility to a user.
                prioritizedAttributeMask |= BusyBoxSetupInstanceAttributes.Environment;
            }

            query = query
                .OrderByDescending(x => (x.Attributes & prioritizedAttributeMask) != 0)
                .ThenByDescending(x => x.Version);
        }

        return query;
    }

    static IEnumerable<IBusyBoxSetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions, BusyBoxDiscoveryOptions options)
    {
        return
            EnumerateSetupDescriptors(versions, options)
            .Select(descriptor => ResolveSetupDescriptor(descriptor))
            .Where(descriptor => descriptor.InstallationPath != null)
            // One installation directory can contain multiple setup instances
            // (one BusyBox file = one instance).
            .GroupBy(descriptor => Path.Combine(descriptor.InstallationPath!, descriptor.ProductPath), FileSystem.PathEquivalenceComparer)
            .Select(group => BusyBoxSetupInstance.TryCreate(group, versions))
            .Where(instance => instance != null)!;
    }

    static BusyBoxSetupDescriptor ResolveSetupDescriptor(in BusyBoxSetupDescriptor descriptor)
    {
        if (descriptor.InstallationPath is null &&
            TryResolveInstallationPath(descriptor, out string? installationPath, out string? productPath))
        {
            return
                descriptor with
                {
                    InstallationPath = installationPath,
                    ProductPath = productPath
                };
        }
        else
        {
            return descriptor;
        }

        static bool TryResolveInstallationPath(
            in BusyBoxSetupDescriptor descriptor,
            [MaybeNullWhen(false)] out string installationPath,
            [MaybeNullWhen(false)] out string productPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Pal.Windows.TryResolveInstallationPath(descriptor, out installationPath, out productPath))
                    return true;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
#if NETCOREAPP
                RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ||
#endif
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (Pal.Unix.TryResolveInstallationPath(descriptor, out installationPath, out productPath))
                    return true;
            }

            installationPath = Empty.Nullify(Path.GetDirectoryName(descriptor.ProductPath));
            if (installationPath != null)
            {
                productPath = Path.GetFileName(descriptor.ProductPath);
                return true;
            }
            else
            {
                productPath = default;
                return false;
            }
        }
    }

    static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(Interval<Version> versions, BusyBoxDiscoveryOptions options)
    {
        IEnumerable<BusyBoxSetupDescriptor> osDescriptors;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            osDescriptors = Pal.Windows.EnumerateSetupDescriptors(options);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
#if NETCOREAPP
            RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ||
#endif
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            osDescriptors = Pal.Unix.EnumerateSetupDescriptors();
        }
        else
        {
            osDescriptors = [];
        }

        foreach (var i in osDescriptors)
            yield return i;

        if ((options & (BusyBoxDiscoveryOptions.NoPath | BusyBoxDiscoveryOptions.NoEnvironment)) == 0)
        {
            foreach (string path in CommandShell.Where("busybox"))
            {
                yield return new(GetRealPath(path))
                {
                    Attributes = BusyBoxSetupInstanceAttributes.Path
                };
            }
        }
    }

    static string GetRealPath(string path) => FileSystem.GetRealPath(path);
}
