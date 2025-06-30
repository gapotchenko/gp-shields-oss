// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
// Portions © Homebrew Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion : IComparable, IComparable<BrewVersion>
{
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

        for (int li = 0, ri = 0, n = Math.Max(leftComponents.Count, rightComponents.Count); li < n;)
        {
            var l = GetComponent(leftComponents, li);
            var r = GetComponent(rightComponents, ri);

            switch (l is BrewVersionComponent.Numeric, r is BrewVersionComponent.Numeric)
            {
                case (true, false):
                    if (l != BrewVersionComponent.Null.Instance)
                        return 1;
                    ++li;
                    break;

                case (false, true):
                    if (r != BrewVersionComponent.Null.Instance)
                        return -1;
                    ++ri;
                    break;

                default:
                    if (l.CompareTo(r) is var comparison and not 0)
                        return comparison;
                    ++li;
                    ++ri;
                    break;
            }
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
}
