// Gapotchenko.Shields.Canonical.Snap.Management
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Canonical.Snap.Deployment;

namespace Gapotchenko.Shields.Canonical.Snap.Management;

/// <summary>
/// Provides operations for Canonical Snap management.
/// </summary>
public interface ISnapManager
{
    /// <summary>
    /// Enumerates installed snap packages.
    /// </summary>
    /// <inheritdoc cref="EnumeratePackages(string?, SnapPackageListingOptions)"/>
    IEnumerable<SnapPackageName> EnumeratePackages(SnapPackageListingOptions options = default);

    /// <summary>
    /// Enumerates installed snap packages with the specified identifier.
    /// </summary>
    /// <param name="packageId">
    /// The optional package identifier to enumerate the packages for.
    /// The <see langword="null"/> value instructs to enumerate all of them.
    /// </param>
    /// <param name="options">The options.</param>
    /// <returns>A sequence of installed snap packages.</returns>
    IEnumerable<SnapPackageName> EnumeratePackages(string? packageId, SnapPackageListingOptions options = default);

    /// <summary>
    /// Gets a directory path for the specified package.
    /// </summary>
    /// <param name="name">The package name.</param>
    /// <returns>The package directory path.</returns>
    /// <exception cref="DirectoryNotFoundException">Snap package directory not found.</exception>
    string GetPackagePath(SnapPackageName name);

    /// <summary>
    /// Tries to gets a directory path for the specified package.
    /// </summary>
    /// <param name="name">The package name.</param>
    /// <returns>
    /// The package directory path,
    /// or <see langword="null"/> if the path cannot be found.
    /// </returns>
    string? TryGetPackagePath(SnapPackageName name);

    /// <summary>
    /// Gets the setup instance of snap associated with this manager.
    /// </summary>
    ISnapSetupInstance Setup { get; }
}
