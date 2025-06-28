// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Text;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Represents a Homebrew package version.
/// </summary>
public sealed partial record BrewVersion : IComparable<BrewVersion>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewVersion"/> class with a specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    public BrewVersion(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        m_Value = value;
    }

    /// <summary>
    /// Gets a value indicating whether the version represents a head SCM version.
    /// </summary>
    public bool IsHead => m_Value.StartsWith(HeadPrefix, StringComparison.Ordinal);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    const string HeadPrefix = "HEAD";

    /// <summary>
    /// Gets a SCM commit information.
    /// </summary>
    /// <value>
    /// The commit information or <see langword="null"/> if the information is absent.
    /// </value>
    public string? Commit =>
        IsHead && m_Value[HeadPrefix.Length] == '-'
            ? m_Value[(HeadPrefix.Length + 1)..]
            : null;

    #region Comparison

    /// <inheritdoc/>
    public int CompareTo(BrewVersion? other)
    {
        if (other is null)
            return 1;
        if (m_Value.Equals(other.m_Value, StringComparison.Ordinal))
            return 0;
        if (IsHead && !other.IsHead)
            return 1;
        if (!IsHead && other.IsHead)
            return -1;

        var leftComponents = Components;
        var rightComponents = other.Components;
        int n = Math.Max(leftComponents.Count, rightComponents.Count);

        for (int i = 0; i < n; i++)
        {
            var a = GetComponent(leftComponents, i);
            var b = GetComponent(rightComponents, i);

            int? comparison = a.TryCompareTo(b);
            if (comparison == 0)
                continue;

            if (a is NumericComponent && b is not NumericComponent)
                return 1;
            if (a is not NumericComponent && b is NumericComponent)
                return -1;

            if (comparison.HasValue && comparison != 0)
                return comparison.Value;
        }

        return 0;
    }

    #endregion

    #region Equality

    /// <inheritdoc/>
    public bool Equals(BrewVersion? other) => CompareTo(other) == 0;

    /// <inheritdoc/>
    public override int GetHashCode() => Components.Select(x => x.GetHashCode()).Aggregate(HashCode.Combine);

    #endregion

    /// <inheritdoc/>
    public override string ToString() => m_Value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string m_Value;
}
