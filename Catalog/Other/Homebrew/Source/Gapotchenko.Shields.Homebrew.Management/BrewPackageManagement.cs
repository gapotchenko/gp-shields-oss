// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Text;

namespace Gapotchenko.Shields.Homebrew.Management;

sealed class BrewPackageManagement(IBrewManager manager, string storagePath) : IBrewPackageManagement
{
    public IEnumerable<BrewPackage> EnumeratePackages(BrewPackageEnumerationOptions options) => EnumeratePackages(null, options);

    public IEnumerable<BrewPackage> EnumeratePackages(string? name, BrewPackageEnumerationOptions options)
    {
        if (name is not null)
            BrewPackage.ValidateName(name);

        return
            EnumeratePackageDirectories(name)
            .SelectMany(
                packagePath =>
                {
                    var query = EnumeratePackageVersions(packagePath);
                    // TODO: is this a correct way to determine the current package version?
                    if ((options & (BrewPackageEnumerationOptions.Top | BrewPackageEnumerationOptions.Current)) != 0)
                        query = query.OrderByDescending(package => package.Version).Take(1);
                    return query;
                });
    }

    static IEnumerable<BrewPackage> EnumeratePackageVersions(string packagePath)
    {
        foreach (string versionPath in Directory.EnumerateDirectories(packagePath))
        {
            if (TryParsePackage(packagePath, versionPath, out var package))
                yield return package;
        }
    }

    static bool TryParsePackage(string packagePath, string versionPath, out BrewPackage package)
    {
        string versionName = Path.GetFileName(versionPath);
        if (versionName.StartsWith('.'))
        {
            // A special directory.
            package = default;
            return false;
        }

        if (!BrewVersion.TryParse(versionName, out var version))
        {
            Debug.Fail($"Cannot parse the version of a Homebrew package in '{versionPath}' directory.");
            package = default;
            return false;
        }

        package = new BrewPackage(Path.GetFileName(packagePath), version);
        return true;
    }

    IEnumerable<string> EnumeratePackageDirectories(string? name)
    {
        if (name is not null)
        {
            if (name is not [] && !IsReservedPackageName(name))
            {
                string path = Path.Combine(storagePath, name);
                if (Directory.Exists(path))
                    yield return path;
            }
        }
        else
        {
            foreach (string path in Directory.EnumerateDirectories(storagePath))
            {
                string directoryName = Path.GetFileName(path);
                if (!directoryName.StartsWith('.') && !IsReservedPackageName(directoryName))
                    yield return path;
            }
        }
    }

    static bool IsReservedPackageName(string name) => false;

    public string GetPackagePath(BrewPackage package)
    {
        return
            TryGetPackagePath(package) ??
            throw new DirectoryNotFoundException(
                string.Format(
                    "Homebrew package directory is not found for '{0}'.",
                    package));
    }

    public string? TryGetPackagePath(BrewPackage package)
    {
        string path = Path.Combine(storagePath, package.Name, package.Version.ToString());
        return Directory.Exists(path) ? path : null;
    }

    public IBrewManager Manager => manager;
}
