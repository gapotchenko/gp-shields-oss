// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using System.Text;

namespace Gapotchenko.Shields.BusyBox.Deployment;

/// <summary>
/// Provides static methods for working with BusyBox setup instances.
/// </summary>
public static class BusyBoxSetupInstance
{
    /// <summary>
    /// Opens a BusyBox setup instance at the specified path.
    /// </summary>
    /// <param name="path">The path to open a BusyBox setup instance at.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>The opened BusyBox setup instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    /// <exception cref="BusyBoxDeploymentException">Cannot open a BusyBox setup instance at the specified path.</exception>
    public static IBusyBoxSetupInstance Open(string path, BusyBoxDiscoveryOptions options = default) =>
        TryOpen(path, options) ??
        throw new BusyBoxDeploymentException("Cannot open a BusyBox setup instance at the specified path.");

    /// <summary>
    /// Tries to open a BusyBox setup instance at the specified directory path of the file system view.
    /// </summary>
    /// <param name="path">The path to open a BusyBox setup instance at.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>
    /// The opened BusyBox setup instance
    /// or <see langword="null"/> if <paramref name="path"/> does not contain it.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    public static IBusyBoxSetupInstance? TryOpen(string path, BusyBoxDiscoveryOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(path);

        return BusyBoxDeployment.EnumerateSetupInstances(path, ValueInterval.Infinite<Version>()).FirstOrDefault();
    }

    internal static IBusyBoxSetupInstance? TryCreate(
        IEnumerable<BusyBoxSetupDescriptor> descriptors,
        Interval<Version> versions)
    {
        descriptors = descriptors.Memoize();

        var primaryDescriptor = descriptors.First();
        string installationPath = primaryDescriptor.InstallationPath!;
        string productPath = primaryDescriptor.ProductPath;

        string busyBoxPath = Path.Combine(installationPath, productPath);
        if (!File.Exists(busyBoxPath))
            return null;

        Lazy<Version> version =
            descriptors.Select(x => x.Version).FirstOrDefault(x => x != null) is { } determinedVersion
                ?
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
                new(determinedVersion)
#else
                new(() => determinedVersion)
#endif
                : new(() => GetVersion(busyBoxPath));

        if (!versions.IsInfinite && !versions.Contains(version.Value))
            return null;

        string? manufacturerVersion = descriptors.Select(x => x.ManufacturerVersion).FirstOrDefault(x => x != null);

        return new BusyBoxSetupInstanceImpl(
            installationPath,
            productPath,
            descriptors.Select(x => x.Architecture).FirstOrDefault(x => x.HasValue) ?? RuntimeInformation.OSArchitecture,
            version,
            manufacturerVersion,
            descriptors.Select(x => x.Attributes).Aggregate((a, b) => a | b));
    }

    static Version GetVersion(string busyBoxPath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(busyBoxPath);
            return new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart);
        }
        else
        {
            using var stream = File.OpenRead(busyBoxPath);
            var version = TryGetVersionFromStream(stream);
            return version ?? throw new BusyBoxDeploymentException("Cannot determine BusyBox version.");
        }
    }

    static Version? TryGetVersionFromStream(Stream stream)
    {
        using var enumerator = stream.AsEnumerable().GetEnumerator();

        for (; ; )
        {
            // Search for version marker.
            int j = enumerator.Rest().IndexOf(Encoding.ASCII.GetBytes("BusyBox v"));
            if (j == -1)
                return null; // EOF

            // Move to the next significant symbol, rectifying 'IndexOf' implementation variations.
            if (enumerator.Current == 'v')
            {
                if (!enumerator.MoveNext())
                    return null; // EOF
            }

            var sb = new StringBuilder();
            do
            {
                char c = (char)enumerator.Current;
                if (!(c <= 0x7f && (c is '.' || char.IsDigit(c))))
                    break;
                sb.Append(c);
            }
            while (enumerator.MoveNext());

            if (sb.Length == 0)
                continue; // invalid data

            if (!Version.TryParse(sb.ToString(), out var version))
                continue; // invalid data

            return version;
        }
    }
}
