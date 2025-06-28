// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Represents a component of the Homebrew package version.
/// </summary>
public abstract class BrewVersionComponent : IComparable<BrewVersionComponent>, IEquatable<BrewVersionComponent>
{
    /// <summary>
    /// Gets a Homebrew package version component from the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Homebrew package version component.</returns>
    /// <exception cref="ArgumentException">Cannot match a component pattern.</exception>
    public static BrewVersionComponent From(string? value)
    {
        if (value is null)
            return Null.Instance;
        else if (Alpha.Regex.IsMatch(value))
            return new Alpha(value);
        if (Beta.Regex.IsMatch(value))
            return new Beta(value);
        else if (ReleaseCandidate.Regex.IsMatch(value))
            return new ReleaseCandidate(value);
        else if (Prerelease.Regex.IsMatch(value))
            return new Prerelease(value);
        else if (Patch.Regex.IsMatch(value))
            return new Patch(value);
        else if (Post.Regex.IsMatch(value))
            return new Post(value);
        else if (Numeric.Regex.IsMatch(value))
            return new Numeric(int.Parse(value, NumberStyles.Integer));
        else if (Text.Regex.IsMatch(value))
            return new Text(value);
        else
            throw new ArgumentException("Cannot match a component pattern.", nameof(value));
    }

    /// <summary>
    /// Gets a value stored in the component, if any.
    /// </summary>
    public object? Value => GetObjectValue();

    /// <summary>
    /// Gets an <see cref="object"/> representation of the value stored in the component, if any.
    /// </summary>
    protected abstract object? GetObjectValue();

    #region Comparison

    /// <inheritdoc/>
    public abstract int CompareTo(BrewVersionComponent? other);

    public static bool operator <(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null
            ? right is not null
            : left.CompareTo(right) < 0;

    public static bool operator <=(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null ||
        left.CompareTo(right) <= 0;

    public static bool operator >(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is not null &&
        left.CompareTo(right) > 0;

    public static bool operator >=(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null
            ? right is null
            : left.CompareTo(right) >= 0;

    #endregion

    #region Equality

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is BrewVersionComponent component &&
        Equals(component);

    /// <inheritdoc/>
    public bool Equals(BrewVersionComponent? other) => CompareTo(other) == 0;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        throw new InvalidOperationException("Should be implemented in a derived class.");

    public static bool operator ==(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null
            ? right is null
            : left.Equals(right);

    public static bool operator !=(BrewVersionComponent? left, BrewVersionComponent? right) => !(left == right);

    #endregion

    /// <inheritdoc/>
    public override string? ToString() => Value?.ToString();

    #region Definitions

    /// <summary>
    /// Represents an alpha component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class Alpha(string value) : Composite(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Alpha alpha => Revision.CompareTo(alpha.Revision),
                Beta or ReleaseCandidate or Prerelease or Patch or Post => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("alpha[0-9]*|a[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a beta component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class Beta(string value) : Composite(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Beta beta => Revision.CompareTo(beta.Revision),
                Alpha => 1,
                Prerelease or ReleaseCandidate or Patch or Post => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("beta[0-9]*|b[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a prerelease component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class Prerelease(string value) : Composite(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Prerelease pre => Revision.CompareTo(pre.Revision),
                Alpha or Beta => 1,
                ReleaseCandidate or Patch or Post => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("pre[0-9]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a release candidate component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class ReleaseCandidate(string value) : Composite(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                ReleaseCandidate rc => Revision.CompareTo(rc.Revision),
                Alpha or Beta or Prerelease => 1,
                Patch or Post => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("rc[0-9]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a patch component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class Patch(string value) : Composite(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Patch patch => Revision.CompareTo(patch.Revision),
                Alpha or Beta or ReleaseCandidate or Prerelease => 1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("p[0-9]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a post component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class Post(string value) : Composite(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Post post => Revision.CompareTo(post.Revision),
                Alpha or Beta or ReleaseCandidate or Prerelease => 1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new(".post[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a composite component of the Homebrew package version.
    /// </summary>
    /// <param name="value">The string value.</param>
    public abstract class Composite(string value) : Text(value)
    {
        /// <summary>
        /// Gets a revision number stored in the component.
        /// </summary>
        public int Revision { get; } = int.TryParse(Numeric.Regex.Match(value).Value, out int result) ? result : 0;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Revision);
    }

    /// <summary>
    /// Represents a text component of the Homebrew package version.
    /// </summary>
    public class Text : BrewVersionComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class with a specified string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public Text(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            Value = value;
        }

        /// <summary>
        /// Gets a string value stored in the component.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public new string Value { get; }

        /// <inheritdoc/>
        protected override object? GetObjectValue() => Value;

        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Text sc => StringComparer.Ordinal.Compare(Value, sc.Value),
                Numeric or Null => -Math.Sign(other.CompareTo(this)),
                null => 1,
                _ => throw new InvalidOperationException()
            };

        /// <inheritdoc/>
        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        internal static readonly Regex Regex = new("[a-z]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a numeric component of the Homebrew package version.
    /// </summary>
    /// <param name="value">The numeric value.</param>
    public sealed class Numeric(int value) : BrewVersionComponent
    {
        /// <summary>
        /// Gets a numeric value stored in the component.
        /// </summary>
        public new int Value { get; } = value;

        /// <inheritdoc/>
        protected override object? GetObjectValue() => Value;

        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Numeric nc => Value.CompareTo(nc.Value),
                Text or null => 1,
                Null => -Math.Sign(other.CompareTo(this)),
                _ => throw new InvalidOperationException()
            };

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Value);

        internal static readonly Regex Regex = new("[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a null component of the Homebrew package version.
    /// </summary>
    public sealed class Null : BrewVersionComponent
    {
        /// <summary>
        /// Gets the <see cref="Null"/> instance.
        /// </summary>
        public static readonly Null Instance = new();

        Null()
        {
        }

        /// <inheritdoc/>
        protected override object? GetObjectValue() => null;

        /// <inheritdoc/>
        public override int CompareTo(BrewVersionComponent? other) =>
            other switch
            {
                Null => 0,
                Numeric nc => nc.Value == 0 ? 0 : -1,
                Alpha or Beta or Prerelease or ReleaseCandidate or null => 1,
                _ => -1
            };

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(0);

        /// <inheritdoc/>
        public override string ToString() => "<Null>";
    }

    #endregion
}

