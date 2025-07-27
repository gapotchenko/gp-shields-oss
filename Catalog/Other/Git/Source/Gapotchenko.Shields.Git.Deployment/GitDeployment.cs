// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Diagnostics;
using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Git.Deployment;

/// <summary>
/// Provides deployment discovery services for Git version control system.
/// </summary>
public static partial class GitDeployment
{
    /// <inheritdoc cref="EnumerateSetupInstances(ValueInterval{Version}, GitDiscoveryOptions)"/>
    public static IEnumerable<IGitSetupInstance> EnumerateSetupInstances(GitDiscoveryOptions options = default) =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>(), options);

    /// <summary>
    /// Enumerates setup instances of Git.
    /// By default, the instances are sorted by the product version and the newest versions come first.
    /// </summary>
    /// <param name="versions">The interval of Git versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>A sequence of discovered setup instances of Git.</returns>
    public static IEnumerable<IGitSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        GitDiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(GitVersion.NaturalizeInterval(versions), options);

        query = query.Memoize();
        if (query.CountIsAtLeast(2))
        {
            // Sort only if there are two or more instances.
            // This precaution is necessary to avoid potentially expensive version retrieving operations
            // when they are not strictly needed.

            var prioritizedAttributeMask = GitSetupInstanceAttributes.None;
            if ((options & GitDiscoveryOptions.EnvironmentInvariant) == 0)
            {
                // Prefer setup instances deducted from the environment
                // because it gives a configuration flexibility to a user.
                prioritizedAttributeMask |= GitSetupInstanceAttributes.Environment;
            }

            query = query
                .OrderByDescending(x => (x.Attributes & prioritizedAttributeMask) != 0)
                .ThenByDescending(x => x.Version);
        }

        return query;
    }

    static IEnumerable<IGitSetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions, GitDiscoveryOptions options)
    {
        return
            EnumerateSetupDescriptors(versions, options)
            .Select(descriptor => ResolveInstallationPath(descriptor))
            .Where(descriptor => descriptor.InstallationPath != null)
            .GroupBy(x => x.InstallationPath!, FileSystem.PathEquivalenceComparer)
            .Select(g => GitSetupInstance.TryCreate(g.Key, g, versions))
            .Where(x => x != null)!;
    }

    static GitSetupDescriptor ResolveInstallationPath(in GitSetupDescriptor descriptor)
    {
        if (descriptor.InstallationPath is null &&
            TryDetermineInstallationPath(descriptor, out string? installationPath, out string? productPath))
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

        static bool TryDetermineInstallationPath(
            in GitSetupDescriptor descriptor,
            [MaybeNullWhen(false)] out string installationPath,
            [MaybeNullWhen(false)] out string productPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Pal.Windows.TryDetermineInstallationPath(descriptor, out installationPath, out productPath))
                    return true;
            }

            installationPath = default;
            productPath = default;
            return false;
        }
    }

    static IEnumerable<GitSetupDescriptor> EnumerateSetupDescriptors(Interval<Version> versions, GitDiscoveryOptions options)
    {
        IEnumerable<GitSetupDescriptor> osDescriptors;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            osDescriptors = Pal.Windows.EnumerateSetupDescriptors(versions);
        else
            osDescriptors = [];

        foreach (var i in osDescriptors)
            yield return i;

        if ((options & (GitDiscoveryOptions.NoPath | GitDiscoveryOptions.NoEnvironment)) == 0)
        {
            foreach (string path in CommandShell.Where("git"))
                yield return new(GetRealPath(path)) { Attributes = GitSetupInstanceAttributes.Path };
        }
    }

    static string GetRealPath(string path) => FileSystem.GetRealPath(path);
}
