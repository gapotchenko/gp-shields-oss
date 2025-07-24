// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.Shields.Microsoft.Wsl.Deployment;

namespace Gapotchenko.Shields.Microsoft.Wsl.Runtime;

/// <summary>
/// Provides information about a running instance of WSL.
/// </summary>
public interface IWslRunningInstance
{
    /// <summary>
    /// Gets the name of Linux distribution.
    /// </summary>
    /// <remarks>
    /// For example: "Ubuntu".
    /// </remarks>
    string DistributionName { get; }

    /// <summary>
    /// Gets a setup instance that corresponds to the running instance of WSL.
    /// </summary>
    IWslSetupInstance Setup { get; }
}
