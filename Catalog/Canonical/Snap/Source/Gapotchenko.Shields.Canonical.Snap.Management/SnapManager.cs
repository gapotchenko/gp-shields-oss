// Gapotchenko.Shields.Canonical.Snap.Management
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Canonical.Snap.Deployment;
using System.Globalization;

namespace Gapotchenko.Shields.Canonical.Snap.Management;

sealed class SnapManager(ISnapSetupInstance setupInstance) : ISnapManager
{
    public IEnumerable<SnapPackageName> EnumeratePackages(SnapPackageListingOptions options) =>
        EnumeratePackages(null, options);

    public IEnumerable<SnapPackageName> EnumeratePackages(string? packageId, SnapPackageListingOptions options)
    {
        if (packageId is not null)
            SnapPackageName.ValidateId(packageId);

        foreach (var packagePath in EnumeratePackageDirectories(packageId))
        {
            if ((options & SnapPackageListingOptions.Current) != 0)
            {
                var revisionPath = Path.Combine(packagePath, "current");
                if (Directory.Exists(revisionPath))
                {
                    if (TryGetSnapPackageName(packagePath, FileSystem.GetRealPath(revisionPath), out var packageName))
                        yield return packageName;
                }
                else
                {
                    Debug.Fail($"The current revision of a snap package is absent in '{packagePath}' directory.");
                }
            }
            else
            {
                foreach (var revisionPath in Directory.EnumerateDirectories(packagePath))
                {
                    if (TryGetSnapPackageName(packagePath, revisionPath, out var packageName))
                        yield return packageName;
                }
            }
        }
    }

    static bool TryGetSnapPackageName(string packagePath, string revisionPath, out SnapPackageName packageName)
    {
        string revisionName = Path.GetFileName(revisionPath);

        switch (revisionName)
        {
            // Reserved revisions.
            case "current":
                packageName = default;
                return false;

            default:
                break;
        }

        if (!int.TryParse(revisionName, NumberStyles.None, NumberFormatInfo.InvariantInfo, out var revision))
        {
            Debug.Fail($"Cannot parse the revision of a snap package in '{revisionPath}' directory.");
            packageName = default;
            return false;
        }

        packageName = new SnapPackageName(Path.GetFileName(packagePath), revision);
        return true;
    }

    IEnumerable<string> EnumeratePackageDirectories(string? packageId)
    {
        if (IsReservedPackageId(packageId))
            yield break;

        var installationPath = setupInstance.InstallationPath;
        if (packageId is not null)
        {
            var path = Path.Combine(installationPath, packageId);
            if (Directory.Exists(path))
                yield return path;
        }
        else
        {
            foreach (var path in Directory.EnumerateDirectories(installationPath))
            {
                if (!IsReservedPackageId(Path.GetFileName(path)))
                    yield return path;
            }
        }
    }

    static bool IsReservedPackageId(string? id) =>
        string.Equals(id, "bin", FileSystem.PathComparison);

    public string GetPackagePath(SnapPackageName name) =>
        TryGetPackagePath(name) ??
        throw new DirectoryNotFoundException(
            string.Format(
                "Snap package directory is not found for '{0}'.",
                name));

    public string? TryGetPackagePath(SnapPackageName name)
    {
        string path = Path.Combine(
            setupInstance.InstallationPath,
            name.Id,
            name.Revision.ToString(NumberFormatInfo.InvariantInfo));

        return Directory.Exists(path) ? path : null;
    }

    public ISnapSetupInstance Setup => setupInstance;
}
