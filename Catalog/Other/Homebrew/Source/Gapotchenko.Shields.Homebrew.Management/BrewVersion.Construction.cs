// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewVersion"/> class with a specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    public BrewVersion(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        m_Value = value;
        Components = [.. ParseComponents(value)];
    }
}
