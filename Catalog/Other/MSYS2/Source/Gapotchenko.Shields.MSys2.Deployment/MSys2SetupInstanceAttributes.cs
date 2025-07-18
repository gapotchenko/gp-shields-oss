// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Specifies the attributes of an MSYS2 setup instance.
/// </summary>
[Flags]
public enum MSys2SetupInstanceAttributes
{
    /// <summary>
    /// No attributes.
    /// </summary>
    None = 0,

    /// <summary>
    /// Setup instance has been deducted from the environment
    /// by using an environment variable.
    /// </summary>
    Environment = 1 << 0
}
