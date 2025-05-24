// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User;

partial struct XdgUserDirectory
{
    /// <summary>
    /// Enumerates all known XDG base directories.
    /// </summary>
    /// <returns>
    /// A sequence of <see cref="XdgUserDirectory"/> structures
    /// sorted in ascending alphabetical order by <see cref="XdgUserDirectory.Name"/> property.
    /// </returns>
    public static IEnumerable<XdgUserDirectory> Enumerate() =>
        m_KnownNames.Select(x => new XdgUserDirectory(x));

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
            Desktop.Name,
            Documents.Name,
            Downloads.Name,
            Music.Name,
            Pictures.Name,
            Public.Name,
            Templates.Name,
            Videos.Name
        };
}
