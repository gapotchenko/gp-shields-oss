// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Text;
using System.Text.RegularExpressions;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Represents a Homebrew package version.
/// </summary>
public sealed record BrewVersion : IComparable, IComparable<BrewVersion>
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

    #region Components

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
        i < components.Count ? components[i] : BrewVersionComponent.Null.Instance;

    /// <summary>
    /// Gets the version components.
    /// </summary>
    public IReadOnlyList<BrewVersionComponent> Components => field ??= [.. ParseComponents(m_Value)];

    static IEnumerable<BrewVersionComponent> ParseComponents(string s)
    {
        foreach (Match match in BrewVersionComponent.Regex.Matches(s))
            yield return BrewVersionComponent.From(match.Value);
    }

    #endregion

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
        IsHead && m_Value[HeadPrefix.Length] is '-'
            ? m_Value[(HeadPrefix.Length + 1)..]
            : null;

    #region Parsing

    /// <summary>
    /// Converts the string representation of a Homebrew package version to an equivalent <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="input">A string that contains a Homebrew package version to convert.</param>
    /// <returns>
    /// An object that is equivalent to the Homebrew package version string specified in the <paramref name="input"/> parameter,
    /// or <see langword="null"/> if <paramref name="input"/> parameter is <see langword="null"/>.
    /// </returns>
    /// <exception cref="FormatException"><paramref name="input"/> string has an invalid format.</exception>
    [return: NotNullIfNotNull(nameof(input))]
    public static BrewVersion? Parse(string? input)
    {
        if (input is null)
            return null;

        try
        {
            var version = new BrewVersion(input);
            _ = version.Components;
            return version;
        }
        catch (ArgumentException)
        {
            throw new FormatException("Homebrew version string has an invalid format.");
        }
    }

    #endregion

    #region Comparison

    /// <summary>
    /// Compares the current <see cref="BrewVersion"/> object to a specified object and returns an indication of their relative values.
    /// </summary>
    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            BrewVersion other => CompareTo(other),
            _ => throw new ArgumentException("Argument must be an instance of BrewVersion type.", nameof(obj))
        };

    /// <inheritdoc/>
    public int CompareTo(BrewVersion? other)
    {
        if (ReferenceEquals(this, other))
            return 0;
        if (other is null)
            return 1;
        if (m_Value.Equals(other.m_Value, StringComparison.Ordinal))
            return 0;

        switch (IsHead, other.IsHead)
        {
            case (true, false):
                return 1;
            case (false, true):
                return -1;
            case (true, true):
                return 0;
        }

        var leftComponents = Components;
        var rightComponents = other.Components;
        int n = Math.Max(leftComponents.Count, rightComponents.Count);

        for (int i = 0; i < n; i++)
        {
            var a = GetComponent(leftComponents, i);
            var b = GetComponent(rightComponents, i);

            if (a.CompareTo(b) is var comparison and not 0)
                return comparison;
        }

        return 0;
    }

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersion"/> object is less than
    /// the right specified <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersion"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersion"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(BrewVersion? left, BrewVersion? right) =>
        left is null
            ? right is not null
            : left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersion"/> object is less than or equal to
    /// the right specified <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersion"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersion"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(BrewVersion? left, BrewVersion? right) =>
        left is null ||
        left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersion"/> object is greater than
    /// the right specified <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersion"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersion"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(BrewVersion? left, BrewVersion? right) => right < left;

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersion"/> object is greater than or equal to
    /// the right specified <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersion"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersion"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(BrewVersion? left, BrewVersion? right) => right <= left;

    #endregion

    #region Equality

    /// <inheritdoc/>
    public bool Equals(BrewVersion? other) => CompareTo(other) == 0;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        IsHead
            ? HashCode.Combine(1)
            : Components
            .Where(x => x != BrewVersionComponent.Null.Instance)
            .Select(x => x.GetHashCode()).Aggregate(HashCode.Combine);

    #endregion

    /// <inheritdoc/>
    public override string ToString() => m_Value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string m_Value;
}
