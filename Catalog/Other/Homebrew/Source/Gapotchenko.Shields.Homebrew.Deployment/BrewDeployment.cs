// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Diagnostics;
using Gapotchenko.FX.IO;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Homebrew.Deployment.Utils;

namespace Gapotchenko.Shields.Homebrew.Deployment;

/// <summary>
/// Provides deployment discovery services for Homebrew package manager.
/// </summary>
public static partial class BrewDeployment
{
    /// <inheritdoc cref="EnumerateSetupInstances(ValueInterval{Version}, BrewDiscoveryOptions)"/>
    public static IEnumerable<IBrewSetupInstance> EnumerateSetupInstances(BrewDiscoveryOptions options = default) =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>(), options);

    /// <summary>
    /// Enumerates setup instances of Homebrew.
    /// By default, the instances are sorted by the product version and the newest versions come first.
    /// </summary>
    /// <param name="versions">The interval of Homebrew versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of Homebrew.</returns>
    public static IEnumerable<IBrewSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        BrewDiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(BrewVersion.NaturalizeInterval(versions), options);

        if ((options & BrewDiscoveryOptions.NoSort) == 0)
        {
            var prioritizedAttributeMask = BrewSetupInstanceAttributes.None;
            if ((options & BrewDiscoveryOptions.EnvironmentInvariant) == 0)
            {
                // Prefer setup instances deducted from the environment
                // because it gives a configuration flexibility to a user.
                prioritizedAttributeMask |= BrewSetupInstanceAttributes.Environment;
            }

            query = query
                .OrderByDescending(x => (x.Attributes & prioritizedAttributeMask) != 0)
                .ThenByDescending(x => x.Version);
        }

        return query;
    }

    static IEnumerable<IBrewSetupInstance> EnumerateSetupInstancesCore(
        Interval<Version> versions,
        BrewDiscoveryOptions options)
    {
        return
            EnumerateSetupDescriptors(options)
            .GroupBy(x => x.InstallationPath, FileSystem.PathEquivalenceComparer)
            .Select(g => BrewSetupInstance.TryCreate(g.Key, g, versions))
            .Where(x => x != null)!;
    }

    static IEnumerable<BrewSetupDescriptor> EnumerateSetupDescriptors(BrewDiscoveryOptions options)
    {
        if ((options & BrewDiscoveryOptions.NoEnvironment) == 0)
        {
            foreach (var root in EnumerateEnvironmentRoots())
            {
                yield return
                    new(Path.GetFullPath(root.Prefix))
                    {
                        Attributes = BrewSetupInstanceAttributes.Environment,
                        CellarPath = GetFullPath(root.Cellar),
                        RepositoryPath = GetFullPath(root.Repository)
                    };
            }
        }

        if ((options & (BrewDiscoveryOptions.NoPath | BrewDiscoveryOptions.NoEnvironment)) == 0)
        {
            string productFileName = ProductUtil.GetProductFileName();
            foreach (string root in EnumeratePathRoots(productFileName))
            {
                yield return
                    new(root)
                    {
                        Attributes = BrewSetupInstanceAttributes.Path,
                        ProductFileName = productFileName
                    };
            }
        }

        IEnumerable<BrewSetupDescriptor> osDescriptors;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            osDescriptors = Pal.MacOS.EnumerateSetupDescriptors();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            osDescriptors = Pal.Linux.EnumerateSetupDescriptors();
        else
            osDescriptors = [];

        foreach (var i in osDescriptors)
            yield return i;
    }

    static IEnumerable<(string Prefix, string? Cellar, string? Repository)> EnumerateEnvironmentRoots()
    {
        string? prefix = Environment.GetEnvironmentVariable("HOMEBREW_PREFIX");
        if (Directory.Exists(prefix))
        {
            yield return (
                prefix,
                Empty.Nullify(Environment.GetEnvironmentVariable("HOMEBREW_CELLAR")),
                Empty.Nullify(Environment.GetEnvironmentVariable("HOMEBREW_REPOSITORY")));
        }
    }

    static IEnumerable<string> EnumeratePathRoots(string productFileName) =>
        CommandShell.Which(productFileName)
        .Select(x => Path.GetDirectoryName(GetRealPath(x)))
        .Where(x => Path.GetFileName(x) == "bin" && Directory.Exists(x))
        .Select(x => Path.GetDirectoryName(x)!) // go one level up out of "bin" directory
        .Where(x => !string.IsNullOrEmpty(x));

    [return: NotNullIfNotNull(nameof(path))]
    static string? GetRealPath(string? path)
    {
        // FUTURE
        // If the product can ever be installed by other package managers,
        // this extension point can be used to get real product paths.
        return FileSystem.GetRealPath(path);
    }

    [return: NotNullIfNotNull(nameof(path))]
    static string? GetFullPath(string? path) => path is null ? null : Path.GetFullPath(path);
}
