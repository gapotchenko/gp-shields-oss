// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Provides static methods for working with MSYS2 setup instances.
/// </summary>
public static class MSys2SetupInstance
{
    /// <summary>
    /// Opens an MSYS2 setup instance at the specified directory path.
    /// </summary>
    /// <param name="directoryPath">The directory path to open an MSYS2 setup instance at.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>The opened MSYS2 setup instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    /// <exception cref="MSys2DeploymentException">Cannot open an MSYS2 setup instance at the specified path.</exception>
    public static IMSys2SetupInstance Open(string directoryPath, MSys2DiscoveryOptions options = default) =>
        TryOpen(directoryPath, options) ??
        throw new MSys2DeploymentException("Cannot open an MSYS2 setup instance at the specified path.");

    /// <summary>
    /// Tries to open an MSYS2 setup instance at the specified directory path of the file system view.
    /// </summary>
    /// <param name="directoryPath">The directory path to open an MSYS2 setup instance at.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>
    /// The opened MSYS2 setup instance
    /// or <see langword="null"/> if <paramref name="directoryPath"/> does not contain it.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    public static IMSys2SetupInstance? TryOpen(string directoryPath, MSys2DiscoveryOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);

        return TryCreate(directoryPath, null, MSys2SetupInstanceAttributes.None, options);
    }

    internal static IMSys2SetupInstance? TryCreate(
        string installationPath,
        Version? version,
        MSys2SetupInstanceAttributes attributes,
        MSys2DiscoveryOptions options)
    {
        string productPath = "msys2.exe";
        if (!File.Exists(Path.Combine(installationPath, productPath)))
            return null;

        Lazy<Version> lazyVersion =
            version is not null
                ?
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                new(version)
#else
                new(() => version)
#endif
                : new(() => TryReadVersion(installationPath) ?? new Version(0, 0, 0));

        return new MSys2SetupInstanceImpl(lazyVersion, installationPath, productPath, attributes, options);
    }

    /// <summary>
    /// Tries to read MSYS2 version from a manifest file.
    /// </summary>
    static Version? TryReadVersion(string installationPath)
    {
        // ------------------------------------------------------------------
        // Trying to read from 'components.xml' file
        // ------------------------------------------------------------------

        string filePath = Path.Combine(installationPath, "components.xml");
        if (File.Exists(filePath))
        {
            // The file is only present in the instances that were installed.
            // Portable instances do not have it.

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
        // Trying to read from 'var/log/pacman.log' file
        // ------------------------------------------------------------------

        filePath = Path.Combine(installationPath, "var", "log", "pacman.log");
        if (File.Exists(filePath))
        {
            string? line;
            using (var file = File.OpenText(filePath))
                line = file.ReadLine();

            if (line != null && line.StartsWith('['))
            {
                // Example lines:
                //   - [2025-02-21T09:51:00+0000] [PACMAN] Running 'pacman -Syu --root /d/a/msys2-installer/msys2-installer/_build/newmsys/msys64'
                var m = Regex.Match(line, @"^\[(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\+\d{4})]");
                if (m.Success &&
                    DateTime.TryParse(m.Groups["timestamp"].Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                {
                    if (TryParseVersion(dateTime) is { } version)
                        return version;
                }
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
        else
        {
            // Nothing worked.
            return null;
        }
    }

    static Version? TryParseVersion(DateTime dateTime)
    {
        if (dateTime.Year is >= 2000 and var year)
        {
            // Year based format.
            return new Version(year, dateTime.Month, dateTime.Day);
        }
        else
        {
            // Nothing worked.
            return null;
        }
    }
}
