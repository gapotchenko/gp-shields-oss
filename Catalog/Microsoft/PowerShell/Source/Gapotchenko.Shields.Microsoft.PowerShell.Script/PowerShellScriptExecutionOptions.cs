// Gapotchenko.Shields.Microsoft.PowerShell.Script
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Script;

/// <summary>
/// Defines the options for PowerShell script execution.
/// </summary>
public sealed record PowerShellScriptExecutionOptions
{
    /// <summary>
    /// Gets or initializes a value indicating whether the executed PowerShell script is interactive.
    /// </summary>
    public bool Interactive { get; init; }
}
