// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion : IEmptiable<BrewVersion>
{
    /// <summary>
    /// Gets the empty <see cref="BrewVersion"/> object.
    /// </summary>
    public static BrewVersion Empty { get; } = EmptyFactory.Instance;

    static class EmptyFactory
    {
        public static readonly BrewVersion Instance = new();
    }

    /// <summary>
    /// Initializes an empty instance of the <see cref="BrewVersion"/> record.
    /// </summary>
    BrewVersion()
    {
        m_Version = string.Empty;
        Components = [];
    }

    /// <inheritdoc/>
    public bool IsEmpty => m_Version is [] && Components.All(x => x.IsEmpty);
}
