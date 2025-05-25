// Gapotchenko.Shields.Microsoft.PowerShell
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

/// <summary>
/// Provides information about the setup instance of Microsoft PowerShell.
/// </summary>
public interface IPowerShellSetupInstance
{
    /// <summary>
    /// <para>
    /// Gets the display name (title) of the product installed in this instance.
    /// </para>
    /// <para>
    /// For example: "Windows PowerShell".
    /// </para>
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// <para>
    /// Gets the root installation path.
    /// </para>
    /// <para>
    /// For example: "C:\Windows\System32\WindowsPowerShell\v1.0".
    /// </para>
    /// </summary>
    string InstallationPath { get; }

    /// <summary>
    /// Gets a relative path to the main product executable.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example: "powershell.exe".
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

    /// <summary>
    /// Gets a package reference to the product registered to the instance.
    /// </summary>
    IPowerShellSetupPackageReference Product { get; }
}
