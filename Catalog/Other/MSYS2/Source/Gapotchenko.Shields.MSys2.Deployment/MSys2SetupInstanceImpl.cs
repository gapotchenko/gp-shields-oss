// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Versioning;
using Gapotchenko.Shields.MSys2.Deployment.Utils;

namespace Gapotchenko.Shields.MSys2.Deployment;

sealed class MSys2SetupInstanceImpl(
    Version version,
    string installationPath,
    string productPath,
    MSys2DiscoveryOptions options) :
    IMSys2SetupInstance,
    IFormattable
{
    public string DisplayName
    {
        get
        {
            const string name = "MSYS2";
            return
                version is (0, 0, 0)
                    ? name // version-less
                    : $"{name} {version.Major}-{version.Minor:D2}-{version.Build:D2}";
        }
    }

    public Version Version => version;

    public string InstallationPath { get; } = Path.TrimEndingDirectorySeparator(installationPath);

    public string ProductPath => productPath;

    public string ResolvePath(string? relativePath)
    {
        string path = Path.GetFullPath(Path.Combine(InstallationPath, relativePath ?? string.Empty));
        if (relativePath == null)
            path += Path.DirectorySeparatorChar;
        return path;
    }

    #region Environments

    public IEnumerable<IMSys2Environment> EnumerateEnvironments() =>
        m_CachedEnvironments ??=
        DoEnumerateEnvironments().Memoize(true);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IEnumerable<IMSys2Environment>? m_CachedEnvironments;

    IEnumerable<IMSys2Environment> DoEnumerateEnvironments()
    {
        var query = EnumerateEnvironmentsCore();

        if ((options & MSys2DiscoveryOptions.NoSort) == 0)
        {
            var orderedQuery = query.OrderBy(x => GetEnvironmentPriority(x.Name));

            static int GetEnvironmentPriority(string name) =>
                name switch
                {
                    // "If you are unsure, go with UCRT64" (from https://www.msys2.org/docs/environments).
                    "UCRT64" => 1,
                    "CLANG64" => 2,
                    "CLANG32" => 3,
                    "CLANGARM64" => 4,
                    "MSYS" => 5,
                    "MINGW64" => 6,
                    "MINGW32" => 7,
                    _ => int.MaxValue
                };

            if ((options & MSys2DiscoveryOptions.ArchitectureInvariant) == 0)
            {
                // Prefer environments with a processor architecture identical to the current process.
                // User intent: programmatically use dynamically-loadable modules inside the process.
                var processArchitecture = RuntimeInformation.ProcessArchitecture;
                orderedQuery = orderedQuery.OrderByDescending(x => x.Architecture == processArchitecture);

                // Prefer environments with a processor architecture similar to the host OS.
                // User intent: run executable modules outside the process.
                var osArchitecture = EnvironmentUtil.TryGetPreciseOSArchitecture() ?? processArchitecture;
                orderedQuery = orderedQuery.ThenBy(x => EnvironmentUtil.GetArchitectureSimilarity(x.Architecture, osArchitecture));
            }

            query = orderedQuery;
        }

        return query;
    }

    IEnumerable<IMSys2Environment> EnumerateEnvironmentsCore()
    {
        string basePath = InstallationPath;

        foreach (string productPath in Directory.EnumerateFiles(basePath, "*.exe"))
        {
            string iniFilePath = Path.ChangeExtension(productPath, ".ini");
            if (!File.Exists(iniFilePath))
                continue;

            string? name;
            using (var iniFile = File.OpenText(iniFilePath))
            {
                name =
                    IniUtil.Read(iniFile)
                    .FirstOrDefault(x => (x.Section, x.Key) is (null, "MSYSTEM"))
                    .Value;
            }

            if (name is null)
                continue;

            string prefix =
                name.Equals("MSYS", StringComparison.OrdinalIgnoreCase)
                    ? "usr"
                    : name.ToLowerInvariant();

            string installationPath = Path.Combine(basePath, prefix);
            if (!Directory.Exists(installationPath))
                continue;

            yield return new MSys2Environment(this, installationPath, productPath, name);
        }
    }

    #endregion

    #region Formatting

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format);

    public string ToString(string? format) =>
        format switch
        {
            "G" or null => ToString()!,
            "D" => DisplayName,
            _ => throw new FormatException("Format specifier was invalid.")
        };

    #endregion
}
