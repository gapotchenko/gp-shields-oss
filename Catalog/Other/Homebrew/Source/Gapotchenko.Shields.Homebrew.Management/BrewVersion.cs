// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
// Portions © Homebrew Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Represents a Homebrew package version.
/// </summary>
[ImmutableObject(true)]
public sealed partial record BrewVersion
{
    // This type is partial.
    // For the rest of the implementation, please take a look at the neighboring source files.

    /// <summary>
    /// Gets the major version component.
    /// </summary>
    public BrewVersionComponent Major => GetComponent(0);

    /// <summary>
    /// Gets the minor version component.
    /// </summary>
    public BrewVersionComponent Minor => GetComponent(1);

    /// <summary>
    /// Gets the patch version component.
    /// </summary>
    public BrewVersionComponent Patch => GetComponent(2);

    BrewVersionComponent GetComponent(int i) => GetComponent(Components, i);

    static BrewVersionComponent GetComponent(IReadOnlyList<BrewVersionComponent> components, int i) =>
        i < components.Count ? components[i] : BrewVersionComponent.Empty;

    /// <summary>
    /// Gets components of the version.
    /// </summary>
    public IReadOnlyList<BrewVersionComponent> Components { get; }

    /// <summary>
    /// Gets a value indicating whether the version represents a head SCM version.
    /// </summary>
    public bool IsHead => m_Version.StartsWith(HeadPrefix, StringComparison.Ordinal);

    /// <summary>
    /// Gets SCM commit information.
    /// </summary>
    /// <value>
    /// The commit information, or <see langword="null"/> if the information is absent.
    /// </value>
    public string? Commit =>
        IsHead && m_Version[HeadPrefix.Length] is '-'
            ? m_Version[(HeadPrefix.Length + 1)..]
            : null;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    const string HeadPrefix = "HEAD";

    /// <summary>
    /// A string representation of the version.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string m_Version;
}
