// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.Base;

/// <summary>
/// Represents an XDG base directory.
/// </summary>
public readonly partial struct XdgBaseDirectory
{
    XdgBaseDirectory(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the directory name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns a string representation of this <see cref="XdgBaseDirectory"/>.
    /// </summary>
    /// <returns>The string representation of this <see cref="XdgBaseDirectory"/>.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Creates a <see cref="XdgBaseDirectory"/> structure from the specified name of an XDG base directory.
    /// </summary>
    /// <param name="name">The name of an XDG base directory.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    public static XdgBaseDirectory FromName(string name) =>
        new(name ?? throw new ArgumentNullException(nameof(name)));
}
