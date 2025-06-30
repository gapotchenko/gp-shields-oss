// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
// Portions © Homebrew Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Represents a component of the the <see cref="BrewVersion"/> record.
/// </summary>
[ImmutableObject(true)]
[DebuggerDisplay("Value = {Value}, Type = {GetType().Name,nq}")]
public abstract class BrewVersionComponent : IComparable, IComparable<BrewVersionComponent>, IEquatable<BrewVersionComponent>
{
    #region Construction

    /// <summary>
    /// Creates a version component with the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A version component.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Cannot match a component pattern.</exception>
    public static BrewVersionComponent Create(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return
            TryParse(value) ??
            throw new ArgumentException("Cannot match a component pattern.", nameof(value));
    }

    #endregion

    #region Parsing

    static BrewVersionComponent? TryParse(string? input)
    {
        if (input is null)
            return null;
        if (Alpha.Regex.IsMatch(input))
            return new Alpha(input);
        if (Beta.Regex.IsMatch(input))
            return new Beta(input);
        else if (ReleaseCandidate.Regex.IsMatch(input))
            return new ReleaseCandidate(input);
        else if (Prerelease.Regex.IsMatch(input))
            return new Prerelease(input);
        else if (Patch.Regex.IsMatch(input))
            return new Patch(input);
        else if (Post.Regex.IsMatch(input))
            return new Post(input);
        else if (Numeric.Regex.IsMatch(input))
            return int.TryParse(input, out int value) ? new Numeric(value) : null;
        else if (Text.Regex.IsMatch(input))
            return new Text(input);
        else
            return null;
    }

    #endregion

    #region Comparison

    /// <summary>
    /// Compares the current <see cref="BrewVersionComponent"/> object to a specified object and returns an indication of their relative values.
    /// </summary>
    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            BrewVersionComponent other => CompareTo(other),
            _ => throw new ArgumentException("Argument must be an instance of BrewVersionComponent type.", nameof(obj))
        };

    /// <inheritdoc/>
    public abstract int CompareTo(BrewVersionComponent? other);

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersionComponent"/> object is less than
    /// the right specified <see cref="BrewVersionComponent"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersionComponent"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersionComponent"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null
            ? right is not null
            : left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersionComponent"/> object is less than or equal to
    /// the right specified <see cref="BrewVersionComponent"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersionComponent"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersionComponent"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null ||
        left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersionComponent"/> object is greater than
    /// the right specified <see cref="BrewVersionComponent"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersionComponent"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersionComponent"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(BrewVersionComponent? left, BrewVersionComponent? right) => right < left;

    /// <summary>
    /// Determines whether the left specified <see cref="BrewVersionComponent"/> object is greater than or equal to
    /// the right specified <see cref="BrewVersionComponent"/> object.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersionComponent"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersionComponent"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >=(BrewVersionComponent? left, BrewVersionComponent? right) => right <= left;

    #endregion

    #region Equality

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is BrewVersionComponent component &&
        Equals(component);

    /// <inheritdoc/>
    public bool Equals(BrewVersionComponent? other) => CompareTo(other) == 0;

    /// <summary>
    /// Returns the hash code for the current version component object.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() =>
        throw new InvalidOperationException("Should be implemented in a derived class.");

    /// <summary>
    /// Determines whether two specified <see cref="BrewVersionComponent"/> objects are equal.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersionComponent"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersionComponent"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> equals <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(BrewVersionComponent? left, BrewVersionComponent? right) =>
        left is null
            ? right is null
            : left.Equals(right);

    /// <summary>
    /// Determines whether two specified <see cref="BrewVersionComponent"/> objects are not equal.
    /// </summary>
    /// <param name="left">The left <see cref="BrewVersionComponent"/> object.</param>
    /// <param name="right">The right <see cref="BrewVersionComponent"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> does not equal <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(BrewVersionComponent? left, BrewVersionComponent? right) => !(left == right);

    #endregion

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal static readonly Regex Regex = new(
        Pattern,
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    const string Pattern = $"{Alpha.Pattern}|{Beta.Pattern}|{Prerelease.Pattern}|{ReleaseCandidate.Pattern}|{Patch.Pattern}|{Post.Pattern}|{Numeric.Pattern}|{Text.Pattern}";

    #region Definitions

    /// <summary>
    /// Represents an alpha component of the <see cref="BrewVersion"/> record.
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "(alpha[0-9]*|a[0-9]+)";
    }

    /// <summary>
    /// Represents a beta component of the <see cref="BrewVersion"/> record.
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "(beta[0-9]*|b[0-9]+)";
    }

    /// <summary>
    /// Represents a prerelease component of the <see cref="BrewVersion"/> record.
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "(pre[0-9]*)";
    }

    /// <summary>
    /// Represents a release candidate component of the <see cref="BrewVersion"/> record.
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "(rc[0-9]*)";
    }

    /// <summary>
    /// Represents a patch component of the <see cref="BrewVersion"/> record.
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "(p[0-9]*)";
    }

    /// <summary>
    /// Represents a post component of the <see cref="BrewVersion"/> record.
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "(.post[0-9]+)";
    }

    /// <summary>
    /// Represents a composite component of the <see cref="BrewVersion"/> record.
    /// </summary>
    /// <param name="value">The string value.</param>
    public abstract class Composite(string value) : Text(value)
    {
        /// <summary>
        /// Gets a revision number stored in the component.
        /// </summary>
        public int Revision { get; } = int.TryParse(m_RevisionRegex.Match(value).Value, out int result) ? result : 0;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Revision);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static readonly Regex m_RevisionRegex = new("[0-9]+", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
    }

    /// <summary>
    /// Represents a text component of the <see cref="BrewVersion"/> record.
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
        public string Value { get; }

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

        /// <inheritdoc/>
        public override string ToString() => Value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "([a-z]+)";
    }

    /// <summary>
    /// Represents a numeric component of the <see cref="BrewVersion"/> record.
    /// </summary>
    /// <param name="value">The numeric value.</param>
    public sealed class Numeric(int value) : BrewVersionComponent
    {
        /// <summary>
        /// Gets a numeric value stored in the component.
        /// </summary>
        public int Value { get; } = value;

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

        /// <inheritdoc/>
        public override string ToString() => Value.ToString(NumberFormatInfo.InvariantInfo);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static new readonly Regex Regex = new($"^{Pattern}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const string Pattern = "([0-9]+)";
    }

    /// <summary>
    /// Represents a null component of the <see cref="BrewVersion"/> record.
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
        public override string? ToString() => null;
    }

    #endregion
}
