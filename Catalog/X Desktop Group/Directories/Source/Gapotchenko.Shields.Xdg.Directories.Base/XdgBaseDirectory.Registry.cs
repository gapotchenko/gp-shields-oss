// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.Base;

partial struct XdgBaseDirectory
{
    /// <summary>
    /// Enumerates all known XDG base directories.
    /// </summary>
    /// <returns>
    /// A sequence of <see cref="XdgBaseDirectory"/> structures
    /// sorted in ascending alphabetical order by <see cref="XdgBaseDirectory.Name"/> property.
    /// </returns>
    public static IEnumerable<XdgBaseDirectory> Enumerate() =>
        m_KnownNames.Select(x => new XdgBaseDirectory(x));

    /// <summary>
    /// Gets a value indicating whether the directory is known.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the directory is known;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsKnown => m_KnownNames.Contains(Name);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static readonly HashSet<string> m_KnownNames =
        new(StringComparer.Ordinal)
        {
            CacheHome.Name,
            ConfigurationDirectories.Name,
            ConfigurationHome.Name,
            DataDirectories.Name,
            DataHome.Name,
            RuntimeDirectory.Name,
            StateHome.Name
        };
}
