// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Provides information about an MSYS2 environment.
/// </summary>
public interface IMSys2Environment
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <remarks>
    /// For example: "UCRT64".
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Gets the architecture.
    /// </summary>
    Architecture Architecture { get; }

    /// <summary>
    /// Gets the root installation path.
    /// </summary>
    /// <remarks>
    /// For example: "C:\msys64\ucrt64".
    /// </remarks>
    string InstallationPath { get; }

    /// <summary>
    /// Gets a relative path to the environment executable.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example: "ucrt64.exe".
    /// </para>
    /// <para>
    /// This path can be then used as an argument to <see cref="IMSys2SetupInstance.ResolvePath(string?)"/> method in order to get the absolute file path.
    /// </para>
    /// </remarks>
    string ProductPath { get; }

    /// <summary>
    /// Gets a setup instance that contains the environment.
    /// </summary>
    IMSys2SetupInstance SetupInstance { get; }

    /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
    string ToString(string? format);
}
