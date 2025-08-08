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
using Gapotchenko.Shields.BusyBox.Deployment.Utils;

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
    /// Enumerates installed setup instances of BusyBox.
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

        return DiscoverSetupInstances(
            EnumerateSetupDescriptors(options),
            versions,
            options);
    }

    /// <summary>
    /// Enumerates portable setup instances of BusyBox at the specified path.
    /// </summary>
    /// <remarks>
    /// By default, BusyBox setup instances are sorted by the product version and edition.
    /// The newest and most suitable versions come first.
    /// </remarks>
    /// <param name="path">The path to discover BusyBox setup instances at.</param>
    /// <param name="versions">The interval of BusyBox versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of BusyBox.</returns>
    public static IEnumerable<IBusyBoxSetupInstance> EnumerateSetupInstances(
        string path,
        ValueInterval<Version> versions,
        BusyBoxDiscoveryOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (versions.IsEmpty)
            return [];

        return DiscoverSetupInstances(
            EnumerateSetupDescriptors(path, options),
            versions,
            options);
    }

    static IEnumerable<IBusyBoxSetupInstance> DiscoverSetupInstances(
        IEnumerable<BusyBoxSetupDescriptor> descriptors,
        in ValueInterval<Version> versions,
        BusyBoxDiscoveryOptions options)
    {
        return OrderSetupInstances(
            EnumerateSetupInstancesCore(
                descriptors,
                BusyBoxVersion.NaturalizeInterval(versions),
                options),
            options);
    }

    static IEnumerable<IBusyBoxSetupInstance> OrderSetupInstances(IEnumerable<IBusyBoxSetupInstance> source, BusyBoxDiscoveryOptions options)
    {
        if ((options & BusyBoxDiscoveryOptions.NoSort) != 0)
            return source;

        var query = source.Memoize();
        if (query.CountIsAtLeast(2))
        {
            // Sort only if there are two or more instances.
            // This precaution is necessary to avoid potentially expensive
            // data retrieving operations when they are not strictly needed.

            var primaryPrioritizedAttributeMask = BusyBoxSetupInstanceAttributes.None;
            var secondaryPrioritizedAttributeMask = BusyBoxSetupInstanceAttributes.None;
            if ((options & BusyBoxDiscoveryOptions.EnvironmentInvariant) == 0)
            {
                // Prefer setup instances deducted from the environment
                // because it gives a configuration flexibility to a user.
                primaryPrioritizedAttributeMask |= BusyBoxSetupInstanceAttributes.Environment;

                var osVersion = Environment.OSVersion;
                if (osVersion.Platform == PlatformID.Win32NT && osVersion.Version >= new Version(10, 0, 18362))
                {
                    // "For 64-bit systems running Windows 10 release 1903 and higher
                    // there's even more advantage in using the busybox64u.exe binary,
                    // as it has support for Unicode."
                    //                               -- https://frippery.org/busybox/
                    secondaryPrioritizedAttributeMask |= BusyBoxSetupInstanceAttributes.Unicode;
                }
            }

            var orderedQuery = query
                .OrderByDescending(x => (x.Attributes & primaryPrioritizedAttributeMask) != 0)
                .ThenByDescending(x => x.Version);

            if ((options & BusyBoxDiscoveryOptions.ArchitectureInvariant) == 0)
            {
                // Prefer setup instances with a processor architecture similar to the host OS.
                // User intent: run executable modules outside the process.
                var osArchitecture = EnvironmentUtil.TryGetPreciseOSArchitecture() ?? RuntimeInformation.ProcessArchitecture;
                orderedQuery = orderedQuery.ThenBy(x => EnvironmentUtil.GetArchitectureSimilarity(x.Architecture, osArchitecture));
            }

            if (secondaryPrioritizedAttributeMask != BusyBoxSetupInstanceAttributes.None)
                orderedQuery = orderedQuery.ThenByDescending(x => (x.Attributes & secondaryPrioritizedAttributeMask) != 0);

            query = orderedQuery;
        }

        return query;
    }

    static IEnumerable<IBusyBoxSetupInstance> EnumerateSetupInstancesCore(
        IEnumerable<BusyBoxSetupDescriptor> descriptors,
        Interval<Version> versions,
        BusyBoxDiscoveryOptions options)
    {
        return descriptors
            .Select(descriptor => ResolveSetupDescriptor(descriptor))
            .Where(descriptor => descriptor.InstallationPath != null)
            // One installation directory can contain multiple setup instances
            // (one BusyBox executable file = one instance).
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

    static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(string path, BusyBoxDiscoveryOptions options)
    {
        IEnumerable<BusyBoxSetupDescriptor> osDescriptors;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            osDescriptors = Pal.Windows.EnumerateSetupDescriptors(path);
        else
            osDescriptors = [];

        foreach (var i in osDescriptors)
            yield return i;

        if (Directory.Exists(path))
        {
            foreach (string busyBoxPath in CommandShell.Where(Path.Combine(path, "busybox")))
                yield return new(GetRealPath(busyBoxPath));
        }
    }

    static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(BusyBoxDiscoveryOptions options)
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
