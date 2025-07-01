// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Provides Homebrew package management operations.
/// </summary>
public interface IBrewPackageManagement
{
    /// <summary>
    /// Enumerates installed packages.
    /// </summary>
    /// <inheritdoc cref="EnumeratePackages(string?, BrewPackageEnumerationOptions)"/>
    IEnumerable<BrewPackage> EnumeratePackages(BrewPackageEnumerationOptions options = default);

    /// <summary>
    /// Enumerates installed packages with the specified identifier.
    /// </summary>
    /// <param name="name">
    /// The optional package name to enumerate the packages for.
    /// The <see langword="null"/> value instructs to enumerate all packages.
    /// </param>
    /// <param name="options">The options.</param>
    /// <returns>A sequence of installed packages.</returns>
    IEnumerable<BrewPackage> EnumeratePackages(string? name, BrewPackageEnumerationOptions options = default);

    /// <summary>
    /// Gets a directory path for the specified package.
    /// </summary>
    /// <param name="package">The package.</param>
    /// <returns>The package directory path.</returns>
    /// <exception cref="DirectoryNotFoundException">Package directory not found.</exception>
    string GetPackagePath(BrewPackage package);

    /// <summary>
    /// Tries to gets a directory path for the specified package.
    /// </summary>
    /// <param name="package">The package.</param>
    /// <returns>
    /// The package directory path,
    /// or <see langword="null"/> if the path cannot be found.
    /// </returns>
    string? TryGetPackagePath(BrewPackage package);

    /// <summary>
    /// Gets the manager this management interface is associated with.
    /// </summary>
    IBrewManager Manager { get; }
}
