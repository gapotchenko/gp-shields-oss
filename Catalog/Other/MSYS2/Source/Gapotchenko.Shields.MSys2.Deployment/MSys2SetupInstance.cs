// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using System.Globalization;
using System.Xml;

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Provides static methods for working with MSYS2 setup instances.
/// </summary>
public static class MSys2SetupInstance
{
    /// <summary>
    /// Opens a MSYS2 setup instance at the specified directory path.
    /// </summary>
    /// <param name="directoryPath">The directory path to open a .NET setup instance at.</param>
    /// <returns>The opened MSYS2 setup instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    /// <exception cref="MSys2DeploymentException">Cannot open a .NET setup instance at the specified path.</exception>
    public static IMSys2SetupInstance Open(string directoryPath) =>
        TryOpen(directoryPath) ??
        throw new MSys2DeploymentException("Cannot open a MSYS2 setup instance at the specified path.");

    /// <summary>
    /// Tries to open a MSYS2 setup instance at the specified directory path of the file system view.
    /// </summary>
    /// <param name="directoryPath">The directory path to open a MSYS2 setup instance at.</param>
    /// <returns>
    /// The opened MSYS2 setup instance
    /// or <see langword="null"/> if <paramref name="directoryPath"/> does not contain it.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    public static IMSys2SetupInstance? TryOpen(string directoryPath)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);

        return TryCreate(directoryPath, null);
    }

    internal static IMSys2SetupInstance? TryCreate(string installationPath, Version? version)
    {
        string productPath = "msys2.exe";
        if (!File.Exists(Path.Combine(installationPath, productPath)))
            return null;

        if (version is null)
        {
            version = TryReadVersion(installationPath);
            if (version is null)
                return null;
        }

        return new MSys2SetupInstanceImpl(version, installationPath, productPath);
    }

    /// <summary>
    /// Tries to read MSYS2 version from a manifest file.
    /// </summary>
    static Version? TryReadVersion(string installationPath)
    {
        // ------------------------------------------------------------------
        // Trying to read from 'components.xml' manifest file
        // ------------------------------------------------------------------

        string filePath = Path.Combine(installationPath, "components.xml");
        if (File.Exists(filePath))
        {
            using var stream = File.OpenRead(filePath);
            try
            {
                // Use a streaming XML parser for better efficiency.
                using var xmlReader = XmlReader.Create(
                    stream,
                    new XmlReaderSettings
                    {
                        // Ignore non-significant data.
                        IgnoreWhitespace = true,
                        IgnoreComments = true,
                        IgnoreProcessingInstructions = true
                    });

                if (!xmlReader.ReadToNextSibling("Packages"))
                    return null;
                xmlReader.ReadStartElement();

                if (!xmlReader.ReadToNextSibling("Package"))
                    return null;
                xmlReader.ReadStartElement();

                if (!xmlReader.ReadToNextSibling("Version"))
                    return null;
                string versionString = xmlReader.ReadElementContentAsString();

                if (TryParseVersion(versionString) is { } version)
                    return version;
            }
            catch (XmlException)
            {
                // Invalid data format.
            }
        }

        // ------------------------------------------------------------------

        // Nothing worked.
        return null;
    }

    internal static Version? TryParseVersion(string? value)
    {
        if (int.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out int versionNumber))
        {
            if (versionNumber is >= 2000_00_00 and < 2100_00_00)
            {
                // Year based format.
                return new Version(
                    versionNumber / 10000,      // year
                    versionNumber / 100 % 100,  // month
                    versionNumber % 100);       // day
            }
            else
            {
                // Unknown format.
                return null;
            }
        }

        // Nothing worked.
        return null;
    }
}
