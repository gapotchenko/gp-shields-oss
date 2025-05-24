// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2024

using Gapotchenko.FX.Text;

namespace Gapotchenko.Shields.Xdg.Directories.Base;

partial struct XdgBaseDirectory : IEquatable<XdgBaseDirectory>
{
    /// <summary>
    /// Determines whether two specified objects are equal.
    /// </summary>
    /// <param name="left">The left object.</param>
    /// <param name="right">The right object.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> equals to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(XdgBaseDirectory left, XdgBaseDirectory right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified objects are not equal.
    /// </summary>
    /// <param name="left">The left object.</param>
    /// <param name="right">The right object.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> does not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(XdgBaseDirectory left, XdgBaseDirectory right) => !left.Equals(right);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is XdgBaseDirectory other &&
        Equals(other);

    /// <inheritdoc/>
    public bool Equals(XdgBaseDirectory other) => Name.Equals(other.Name, StringComparison.Ordinal);

    /// <inheritdoc/>
    public override int GetHashCode() => Name.GetHashCode(StringComparison.Ordinal);
}
