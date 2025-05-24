// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User;

/// <summary>
/// Represents an XDG user directory.
/// </summary>
public readonly partial struct XdgUserDirectory
{
    XdgUserDirectory(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the directory name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns a string representation of this <see cref="XdgUserDirectory"/>.
    /// </summary>
    /// <returns>The string representation of this <see cref="XdgUserDirectory"/>.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Creates a <see cref="XdgUserDirectory"/> structure from the specified name of an XDG user directory.
    /// </summary>
    /// <param name="name">The name of an XDG user directory.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    public static XdgUserDirectory FromName(string name) =>
        new(name ?? throw new ArgumentNullException(nameof(name)));
}
