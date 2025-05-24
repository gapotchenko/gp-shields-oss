// Gapotchenko.Shields.Canonical.Snap.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

/// <summary>
/// Provides information about a setup instance of Canonical Snap.
/// </summary>
public interface ISnapSetupInstance
{
    /// <summary>
    /// Gets the root installation path.
    /// </summary>
    /// <remarks>
    /// For example: "/snap".
    /// </remarks>
    string InstallationPath { get; }

    /// <summary>
    /// Gets a relative path to the main product executable.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example: "/usr/bin/snap".
    /// Which is an absolute path, signifying that the product path can have both relative and absolute forms depending on a particular context.
    /// </para>
    /// <para>
    /// This path then should be used as an argument to <see cref="ResolvePath(string?)"/> method in order to get the absolute file path.
    /// </para>
    /// </remarks>
    string ProductPath { get; }

    /// <summary>
    /// Resolves the relative path to the root path of the instance.
    /// </summary>
    /// <param name="relativePath">The relative path or <see langword="null"/>.</param>
    /// <returns>
    /// The full path to the relative path within the instance.
    /// If the relative path is <see langword="null"/>, the root path will always terminate in a backslash.
    /// </returns>
    string ResolvePath(string? relativePath);

    /// <summary>
    /// Gets the instance attributes.
    /// </summary>
    SnapSetupInstanceAttributes Attributes { get; }
}
