// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Flags for MSYS2 deployment discovery operations.
/// </summary>
[Flags]
public enum MSys2DiscoveryOptions
{
    /// <summary>
    /// No options specified.
    /// The default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Neither sort nor prioritize product setup instances by version or architecture.
    /// </summary>
    NoSort = 1 << 0,

    /// <summary>
    /// Do not prioritize setup instances with processor architecture identical to the current process and host OS architectures.
    /// </summary>
    ArchitectureInvariant = 1 << 1,

    /// <summary>
    /// Do not prioritize setup instances deducted from the environment.
    /// </summary>
    EnvironmentInvariant = 1 << 2,

    /// <summary>
    /// Do not prioritize setup instances by variant factors such as architecture and environment.
    /// </summary>
    Invariant = ArchitectureInvariant | EnvironmentInvariant
}
