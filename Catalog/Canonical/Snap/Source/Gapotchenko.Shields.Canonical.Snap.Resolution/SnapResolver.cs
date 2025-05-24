// Gapotchenko.Shields.Canonical.Snap.Resolution
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.FX.Collections.Generic;
using Gapotchenko.Shields.Canonical.Snap.Deployment;
using Gapotchenko.Shields.Canonical.Snap.Management;
using Gapotchenko.Shields.Canonical.Snap.Resolution.Utils;

namespace Gapotchenko.Shields.Canonical.Snap.Resolution;

/// <summary>
/// Provides package resolution services for Canonical Snap technology.
/// </summary>
public static class SnapResolver
{
    /// <summary>
    /// Gets a real path of the specified file managed by the Snap package manager.
    /// If the file is not managed by the Snap, then the original file path is returned.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns>
    /// The path of the actual file location inside a snap package,
    /// or the original file path if the file is not managed by the Snap package manager.
    /// </returns>
    [return: NotNullIfNotNull(nameof(filePath))]
    public static string? GetRealFilePath(string? filePath)
    {
        if (File.Exists(filePath))
        {
            string? fullFilePath = null;
            return
                SnapDeployment.EnumerateSetupInstances()
                .Select(x => TryGetRealPath(x, fullFilePath ??= Path.GetFullPath(filePath)))
                .FirstOrDefault(x => x is not null) ??
                filePath;
        }
        else
        {
            return filePath;
        }
    }

    static string? TryGetRealPath(ISnapSetupInstance setupInstance, string filePath)
    {
        if (!TryResolveLinkTarget(filePath, out var finalPath, out var penultimatePath))
            return null;

        //Console.WriteLine("Real path of '{0}' is '{1}'.", filePath, finalPath);
        //Console.WriteLine("Penultimate path of '{0}' is '{1}'.", filePath, penultimatePath);

        if (!FileSystem.PathsAreEquivalent(finalPath, setupInstance.ResolvePath(setupInstance.ProductPath)))
            return null;

        //Console.WriteLine("Snap match");

        // ------------------------------------------------------------------

        string packageId;
        string appName;

        var penultimateFileName = Path.GetFileName(penultimatePath);

        // Examples:
        //   - "dotnet-sdk.dotnet"

        int j = penultimateFileName.IndexOf('.');
        if (j == -1)
        {
            appName = packageId = penultimateFileName;
        }
        else
        {
            packageId = penultimateFileName[..j];
            appName = penultimateFileName[(j + 1)..];
        }

        //Console.WriteLine("Snap package ID: {0}", packageId);
        //Console.WriteLine("Snap app name: {0}", appName);

        // ------------------------------------------------------------------

        var snap = SnapManagement.CreateManager(setupInstance);

        var packageName =
            snap.EnumeratePackages(packageId, SnapPackageListingOptions.Current)
            .FirstOrDefault();
        if (packageName == default)
            return null;

        //Console.WriteLine("Snap package: {0}", packageName);

        var packagePath = snap.TryGetPackagePath(packageName);
        if (packagePath is null)
            return null;

        //Console.WriteLine("Snap package path: {0}", packagePath);

        string? command = TryGetAppCommand(packagePath, appName);
        if (command is null)
            return null;

        // ------------------------------------------------------------------

        var commandFilePath = CommandLine.Split(command).FirstOrDefault();
        if (commandFilePath is null)
            return null;

        return Path.Combine(packagePath, commandFilePath);
    }

    static bool TryResolveLinkTarget(
        string linkPath,
        [MaybeNullWhen(false)] out string finalTargetPath,
        [MaybeNullWhen(false)] out string penultimateTargetPath)
    {
#if NET6_0_OR_GREATER
        string? p0 = null, p1 = null;

        for (var p = linkPath; ;)
        {
            var target = File.ResolveLinkTarget(p, false);
            if (target is null)
                break;
            p = target.FullName;
            p1 = p0;
            p0 = p;
        }

        if (p0 is not null)
        {
            finalTargetPath = p0;
            penultimateTargetPath = p1 ?? linkPath;
            return true;
        }
        else
        {
            finalTargetPath = default;
            penultimateTargetPath = default;
            return false;
        }
#else
        // This scenario should never occur in reality
        // because .NET Framework cannot work on platforms other than
        // Windows OS.

        throw new PlatformNotSupportedException();
#endif
    }

    static string? TryGetAppCommand(string packagePath, string appName)
    {
        string manifestFilePath = Path.Combine(packagePath, "meta", "snap.yaml");
        if (!File.Exists(manifestFilePath))
            return null;

        IReadOnlyDictionary<object, object>? manifest;
        using (var file = File.OpenText(manifestFilePath))
            manifest = YamlUtil.ToDictionary(file);
        if (manifest is null)
            return null;

        if (manifest.GetValueOrDefault("apps") is not IReadOnlyDictionary<object, object> apps)
            return null;

        if (apps.GetValueOrDefault(appName) is not IReadOnlyDictionary<object, object> app)
            return null;

        return app.GetValueOrDefault("command") as string;
    }
}
