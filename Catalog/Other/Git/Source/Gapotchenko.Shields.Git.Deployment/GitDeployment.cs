// Gapotchenko.Shields.Git
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
            EnumerateSetupDescriptors(options)
            .Select(descriptor => ResolveSetupDescriptor(descriptor))
            .Where(descriptor => descriptor.InstallationPath != null)
            .GroupBy(descriptor => descriptor.InstallationPath!, FileSystem.PathEquivalenceComparer)
            .Select(group => GitSetupInstance.TryCreate(group.Key, group, versions))
            .Where(instance => instance != null)!;
    }

    static GitSetupDescriptor ResolveSetupDescriptor(in GitSetupDescriptor descriptor)
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
            in GitSetupDescriptor descriptor,
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

            installationPath = default;
            productPath = default;
            return false;
        }
    }

    static IEnumerable<GitSetupDescriptor> EnumerateSetupDescriptors(GitDiscoveryOptions options)
    {
        IEnumerable<GitSetupDescriptor> osDescriptors;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            osDescriptors = Pal.Windows.EnumerateSetupDescriptors();
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

        // TODO: implement package managers support (homebrew)

        if ((options & (GitDiscoveryOptions.NoPath | GitDiscoveryOptions.NoEnvironment)) == 0)
        {
            foreach (string path in CommandShell.Where("git"))
            {
                yield return new(GetRealPath(path))
                {
                    Attributes = GitSetupInstanceAttributes.Path,
                    LibExecPath = Empty.Nullify(Environment.GetEnvironmentVariable("GIT_EXEC_PATH"))
                };
            }
        }
    }

    static string GetRealPath(string path) => FileSystem.GetRealPath(path);
}
