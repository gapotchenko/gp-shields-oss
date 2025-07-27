// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Git.Deployment;

/// <summary>
/// Specifies the attributes of a Git setup instance.
/// </summary>
[Flags]
public enum GitSetupInstanceAttributes
{
    /// <summary>
    /// No attributes.
    /// </summary>
    None = 0,

    /// <summary>
    /// Setup instance has been deducted from the environment by using
    /// <c>GIT_EXEC_PATH</c>
    /// environment variable.
    /// </summary>
    Environment = 1 << 0,

    /// <summary>
    /// Setup instance has been deducted from <c>PATH</c> environment variable.
    /// </summary>
    Path = 1 << 1
}
