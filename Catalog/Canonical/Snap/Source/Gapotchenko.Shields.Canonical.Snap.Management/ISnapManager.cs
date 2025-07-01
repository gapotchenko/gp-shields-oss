// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Canonical.Snap.Deployment;

namespace Gapotchenko.Shields.Canonical.Snap.Management;

/// <summary>
/// Provides Canonical Snap management operations.
/// </summary>
public interface ISnapManager
{
    /// <summary>
    /// Enumerates installed packages.
    /// </summary>
    /// <inheritdoc cref="EnumeratePackages(string?, SnapPackageEnumerationOptions)"/>
    IEnumerable<SnapPackageName> EnumeratePackages(SnapPackageEnumerationOptions options = default);

    /// <summary>
    /// Enumerates installed packages with the specified identifier.
    /// </summary>
    /// <param name="packageId">
    /// The optional package identifier to enumerate the packages for.
    /// The <see langword="null"/> value instructs to enumerate all packages.
    /// </param>
    /// <param name="options">The options.</param>
    /// <returns>A sequence of installed packages.</returns>
    IEnumerable<SnapPackageName> EnumeratePackages(string? packageId, SnapPackageEnumerationOptions options = default);

    /// <summary>
    /// Gets a directory path for the specified package.
    /// </summary>
    /// <param name="name">The package name.</param>
    /// <returns>The package directory path.</returns>
    /// <exception cref="DirectoryNotFoundException">Package directory not found.</exception>
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
    /// Gets the setup instance this management interface is associated with.
    /// </summary>
    ISnapSetupInstance Setup { get; }
}
