// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion
{
    /// <inheritdoc/>
    public override string ToString() => m_Value;
}
