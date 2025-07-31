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

    #region Deduction

    /// <summary>
    /// Disallows environment deduction.
    /// When this option is active, all environment variables are ignored.
    /// </summary>
    NoEnvironment = 1 << 0,

    /// <summary>
    /// Disallows <c>PATH</c> deduction.
    /// When this option is active, <c>PATH</c> environment variable is ignored.
    /// </summary>
    /// <remarks>
    /// <c>PATH</c> environment variable plays a special role in command-line tools discovery.
    /// For more information, see <see href="https://en.wikipedia.org/wiki/PATH_(variable)"/>.
    /// </remarks>
    NoPath = 1 << 1,

    #endregion

    #region Sorting

    /// <summary>
    /// Neither sort nor prioritize product setup instances by version or architecture.
    /// </summary>
    NoSort = 1 << 2,

    /// <summary>
    /// Do not prioritize setup instances with processor architecture identical to the current process and host OS architectures.
    /// </summary>
    ArchitectureInvariant = 1 << 3,

    /// <summary>
    /// Do not prioritize setup instances deducted from the environment.
    /// </summary>
    EnvironmentInvariant = 1 << 4,

    /// <summary>
    /// Do not prioritize setup instances by variant factors such as architecture and environment.
    /// </summary>
    Invariant = ArchitectureInvariant | EnvironmentInvariant

    #endregion
}
