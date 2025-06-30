// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewVersion"/> record with the specified string.
    /// </summary>
    /// <param name="version">A string containing the Homebrew package version.</param>
    /// <exception cref="ArgumentNullException"><paramref name="version"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="version"/> is empty.</exception>
    /// <exception cref="ArgumentException"><paramref name="version"/> has an invalid format.</exception>
    public BrewVersion(string version) :
        this(Model.Create(version))
    {
    }
}
