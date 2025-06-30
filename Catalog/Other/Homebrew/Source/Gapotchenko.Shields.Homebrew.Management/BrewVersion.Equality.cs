// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion
{
    /// <inheritdoc/>
    public bool Equals(BrewVersion? other) => CompareTo(other) == 0;

    /// <summary>
    /// Returns the hash code for the current <see cref="BrewVersion"/> object.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() =>
        IsHead
            ? HashCode.Combine(1)
            : Components
            .Where(x => !x.IsEmpty)
            .Select(x => x.GetHashCode()).Aggregate(HashCode.Combine);
}
