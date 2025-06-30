// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Defines options for Homebrew package enumeration operations.
/// </summary>
[Flags]
public enum BrewPackageEnumerationOptions
{
    /// <summary>
    /// No options are specified.
    /// This is the default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Enumerate only the highest package versions.
    /// </summary>
    Top = 1 << 0
}
