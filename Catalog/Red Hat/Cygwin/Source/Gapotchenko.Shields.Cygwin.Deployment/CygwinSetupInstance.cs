// Gapotchenko.Shields.Cygwin
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Cygwin.Deployment;

/// <summary>
/// Provides static methods for working with Cygwin setup instances.
/// </summary>
public static class CygwinSetupInstance
{
    /// <summary>
    /// Opens a Cygwin setup instance at the specified directory path.
    /// </summary>
    /// <param name="directoryPath">The directory path to open a Cygwin setup instance at.</param>
    /// <returns>The opened Cygwin setup instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    /// <exception cref="CygwinDeploymentException">Cannot open a Cygwin setup instance at the specified path.</exception>
    public static ICygwinSetupInstance Open(string directoryPath) =>
        TryOpen(directoryPath) ??
        throw new CygwinDeploymentException("Cannot open a Cygwin setup instance at the specified path.");

    /// <summary>
    /// Tries to open a Cygwin setup instance at the specified directory path of the file system view.
    /// </summary>
    /// <param name="directoryPath">The directory path to open a Cygwin setup instance at.</param>
    /// <returns>
    /// The opened Cygwin setup instance
    /// or <see langword="null"/> if <paramref name="directoryPath"/> does not contain it.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    public static ICygwinSetupInstance? TryOpen(string directoryPath)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);

        return TryCreate(directoryPath, null);
    }

    internal static ICygwinSetupInstance? TryCreate(string installationPath, Interval<Version>? versions)
    {
        string productPath = "Cygwin.bat";
        if (!File.Exists(Path.Combine(installationPath, productPath)))
            return null;

        string mainModulePath = Path.Combine(installationPath, @"bin\cygwin1.dll");
        if (!File.Exists(mainModulePath))
            return null;

        var versionInfo = FileVersionInfo.GetVersionInfo(mainModulePath);
        if (!Version.TryParse(versionInfo.ProductVersion, out var version))
            return null;
        if (versions?.Contains(version) == false)
            return null;

        return new CygwinSetupInstanceImpl(version, installationPath, productPath);
    }
}
