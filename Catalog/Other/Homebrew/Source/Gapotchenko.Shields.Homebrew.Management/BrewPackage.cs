// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using System.Runtime.CompilerServices;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Represents a Homebrew package.
/// </summary>
public readonly record struct BrewPackage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewPackage"/> record.
    /// </summary>
    /// <param name="name">The package name.</param>
    /// <param name="version">The package version.</param>
    public BrewPackage(string name, BrewVersion version)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ValidateName(name);
        ArgumentNullException.ThrowIfNull(version);

        m_Name = name;
        m_Version = version;
    }

    /// <summary>
    /// Gets or initializes the package name.
    /// </summary>
    public string Name
    {
        get => m_Name;
        init
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            ValidateName(value);

            m_Name = value;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string m_Name;

    /// <summary>
    /// Gets or initializes the package version.
    /// </summary>
    public BrewVersion Version
    {
        get => m_Version;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            m_Version = value;
        }
    }

    readonly BrewVersion m_Version;

    /// <inheritdoc/>
    public override string ToString() => Invariant($"{m_Name}/{m_Version}");

    [StackTraceHidden]
    internal static void ValidateName(
        string name,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
    {
        int j = name.IndexOfAny(['?', '*', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar]);
        if (j != -1)
        {
            throw new ArgumentException(
                string.Format(
                    "The value contains a prohibited symbol '{0}'.",
                    name[j]),
                paramName);
        }
    }
}
