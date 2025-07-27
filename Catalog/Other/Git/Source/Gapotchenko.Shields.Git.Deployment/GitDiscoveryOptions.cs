// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Git.Deployment;

/// <summary>
/// Flags for Git deployment discovery operations.
/// </summary>
[Flags]
public enum GitDiscoveryOptions
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
    /// Do not sort product setup instances by version.
    /// </summary>
    NoSort = 1 << 2,

    /// <summary>
    /// Do not prioritize setup instances deducted from the environment.
    /// </summary>
    EnvironmentInvariant = 1 << 3

    #endregion
}
