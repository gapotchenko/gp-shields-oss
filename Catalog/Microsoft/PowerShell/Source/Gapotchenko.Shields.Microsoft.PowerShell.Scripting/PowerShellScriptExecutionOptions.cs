// Gapotchenko.Shields.Microsoft.PowerShell
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Scripting;

/// <summary>
/// Defines the options for PowerShell script execution.
/// </summary>
public sealed record PowerShellScriptExecutionOptions
{
    /// <summary>
    /// Gets or initializes a value indicating whether the executed PowerShell script is interactive.
    /// </summary>
    /// <remarks>
    /// When the property is set to <see langword="true"/>,
    /// <see cref="InputReader"/> and <see cref="OutputWriter"/> properties should be set to non-null values.
    /// </remarks>
    public bool Interactive { get; init; }

    /// <summary>
    /// Gets or initializes the script input reader.
    /// </summary>
    /// <remarks>
    /// The script input reader should be set when <see cref="Interactive"/> property is <see langword="true"/>.
    /// </remarks>
    public TextReader? InputReader { get; init; }

    /// <summary>
    /// Gets or initializes the script output writer.
    /// </summary>
    /// <remarks>
    /// The script output writer should be set when <see cref="Interactive"/> property is <see langword="true"/>.
    /// </remarks>
    public TextWriter? OutputWriter { get; init; }
}
