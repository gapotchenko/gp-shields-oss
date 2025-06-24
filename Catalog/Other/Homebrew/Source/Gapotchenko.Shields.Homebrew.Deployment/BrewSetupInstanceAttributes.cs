// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Deployment;

/// <summary>
/// Specifies the attributes of a Homebrew package manager setup instance.
/// </summary>
public enum BrewSetupInstanceAttributes
{
    /// <summary>
    /// No attributes.
    /// </summary>
    None = 0,

    /// <summary>
    /// Setup instance has been deducted from the environment by using
    /// <c>HOMEBREW_PREFIX</c>
    /// environment variable.
    /// </summary>
    Environment = 1 << 0,

    /// <summary>
    /// Setup instance has been deducted from <c>PATH</c> environment variable.
    /// </summary>
    Path = 1 << 1
}
