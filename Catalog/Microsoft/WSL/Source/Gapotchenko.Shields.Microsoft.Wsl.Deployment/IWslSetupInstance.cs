// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

/// <summary>
/// Provides information about a setup instance of Microsoft WSL.
/// </summary>
public interface IWslSetupInstance
{
    /// <summary>
    /// Gets the display name (title) of the product installed in this instance.
    /// </summary>
    /// <remarks>
    /// For example: "WSL 2.5.7.0".
    /// </remarks>
    string DisplayName { get; }

    /// <summary>
    /// Gets the installed version of the product instance.
    /// </summary>
    /// <remarks>
    /// For example: <c>2.5.7.0</c>.
    /// </remarks>
    Version Version { get; }

    /// <summary>
    /// Gets the root installation path.
    /// </summary>
    /// <remarks>
    /// For example: "C:\Program Files\WSL".
    /// </remarks>
    string InstallationPath { get; }

    /// <summary>
    /// Gets a relative path to the main product executable.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example: "wsl.exe".
    /// </para>
    /// <para>
    /// This path can be then used as an argument to <see cref="ResolvePath(string?)"/> method in order to get the absolute file path.
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

    /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
    string ToString(string? format);
}
