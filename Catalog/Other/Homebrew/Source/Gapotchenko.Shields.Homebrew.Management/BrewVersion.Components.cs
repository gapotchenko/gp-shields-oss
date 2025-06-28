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

partial record BrewVersion
{
    /// <summary>
    /// Gets the major version component.
    /// </summary>
    public Component Major => GetComponent(0);

    /// <summary>
    /// Gets the minor version component.
    /// </summary>
    public Component Minor => GetComponent(1);

    /// <summary>
    /// Gets the patch version component.
    /// </summary>
    public Component Patch => GetComponent(2);

    Component GetComponent(int i) => GetComponent(Components, i);

    static Component GetComponent(IReadOnlyList<Component> components, int i) =>
        i < components.Count ? components[i] : NullComponent.Instance;

    /// <summary>
    /// Gets the version components.
    /// </summary>
    public IReadOnlyList<Component> Components => field ??= [.. ParseComponents(m_Value)];

    static IEnumerable<Component> ParseComponents(string s)
    {
        foreach (Match match in Regex.Matches(s, RegexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            yield return Component.From(match.Value);
    }

    static readonly string RegexPattern = string.Join("|",
        AlphaComponent.Regex.ToString(),
        BetaComponent.Regex.ToString(),
        PreComponent.Regex.ToString(),
        RCComponent.Regex.ToString(),
        PatchComponent.Regex.ToString(),
        PostComponent.Regex.ToString(),
        NumericComponent.Regex.ToString(),
        StringComponent.Regex.ToString());

    #region Component Definitions

    /// <summary>
    /// Represents an alpha component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class AlphaComponent(string value) : CompositeComponent(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                AlphaComponent alpha => Revision.CompareTo(alpha.Revision),
                BetaComponent or RCComponent or PreComponent or PatchComponent or PostComponent => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("alpha[0-9]*|a[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a beta component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class BetaComponent(string value) : CompositeComponent(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                BetaComponent beta => Revision.CompareTo(beta.Revision),
                AlphaComponent => 1,
                PreComponent or RCComponent or PatchComponent or PostComponent => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("beta[0-9]*|b[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a prerelease component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class PreComponent(string value) : CompositeComponent(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                PreComponent pre => Revision.CompareTo(pre.Revision),
                AlphaComponent or BetaComponent => 1,
                RCComponent or PatchComponent or PostComponent => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("pre[0-9]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a release candidate component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class RCComponent(string value) : CompositeComponent(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                RCComponent rc => Revision.CompareTo(rc.Revision),
                AlphaComponent or BetaComponent or PreComponent => 1,
                PatchComponent or PostComponent => -1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("rc[0-9]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a patch component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class PatchComponent(string value) : CompositeComponent(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                PatchComponent patch => Revision.CompareTo(patch.Revision),
                AlphaComponent or BetaComponent or RCComponent or PreComponent => 1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new("p[0-9]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a post component of the Homebrew package version.
    /// </summary>
    /// <inheritdoc/>
    public sealed class PostComponent(string value) : CompositeComponent(value)
    {
        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                PostComponent post => Revision.CompareTo(post.Revision),
                AlphaComponent or BetaComponent or RCComponent or PreComponent => 1,
                _ => base.CompareTo(other)
            };

        internal static new readonly Regex Regex = new(".post[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a composite component of the Homebrew package version.
    /// </summary>
    /// <param name="value">The string value.</param>
    public abstract class CompositeComponent(string value) : StringComponent(value)
    {
        /// <summary>
        /// Gets a revision number stored in the component.
        /// </summary>
        public int Revision { get; } = int.TryParse(NumericComponent.Regex.Match(value).Value, out int result) ? result : 0;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Revision);
    }

    /// <summary>
    /// Represents a string component of the Homebrew package version.
    /// </summary>
    public class StringComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringComponent"/> class with a specified string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public StringComponent(string value)
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
        public override int CompareTo(Component? other) =>
            other switch
            {
                StringComponent sc => StringComparer.Ordinal.Compare(Value, sc.Value),
                NumericComponent or NullComponent => -Math.Sign(other.CompareTo(this)),
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
    public sealed class NumericComponent(int value) : Component
    {
        /// <summary>
        /// Gets a numeric value stored in the component.
        /// </summary>
        public new int Value { get; } = value;

        /// <inheritdoc/>
        protected override object? GetObjectValue() => Value;

        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                NumericComponent nc => Value.CompareTo(nc.Value),
                StringComponent or null => 1,
                NullComponent => -Math.Sign(other.CompareTo(this)),
                _ => throw new InvalidOperationException()
            };

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Value);

        internal static readonly Regex Regex = new("[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// Represents a null component of the Homebrew package version.
    /// </summary>
    public sealed class NullComponent : Component
    {
        /// <summary>
        /// Gets the <see cref="NullComponent"/> instance.
        /// </summary>
        public static readonly NullComponent Instance = new();

        NullComponent()
        {
        }

        /// <inheritdoc/>
        protected override object? GetObjectValue() => null;

        /// <inheritdoc/>
        public override int CompareTo(Component? other) =>
            other switch
            {
                NullComponent => 0,
                NumericComponent nc => nc.Value == 0 ? 0 : -1,
                AlphaComponent or BetaComponent or PreComponent or RCComponent or null => 1,
                _ => -1
            };

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(0);

        /// <inheritdoc/>
        public override string ToString() => "<Null>";
    }

    /// <summary>
    /// Represents a component of the Homebrew package version.
    /// </summary>
    public abstract class Component : IComparable<Component>, IEquatable<Component>
    {
        /// <summary>
        /// Gets a Homebrew package version component from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A Homebrew package version component.</returns>
        /// <exception cref="ArgumentException">Cannot match a component pattern.</exception>
        public static Component From(string? value)
        {
            if (value is null)
                return NullComponent.Instance;
            else if (AlphaComponent.Regex.IsMatch(value))
                return new AlphaComponent(value);
            if (BetaComponent.Regex.IsMatch(value))
                return new BetaComponent(value);
            else if (RCComponent.Regex.IsMatch(value))
                return new RCComponent(value);
            else if (PreComponent.Regex.IsMatch(value))
                return new PreComponent(value);
            else if (PatchComponent.Regex.IsMatch(value))
                return new PatchComponent(value);
            else if (PostComponent.Regex.IsMatch(value))
                return new PostComponent(value);
            else if (NumericComponent.Regex.IsMatch(value))
                return new NumericComponent(int.Parse(value, NumberStyles.Integer));
            else if (StringComponent.Regex.IsMatch(value))
                return new StringComponent(value);
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
        public abstract int CompareTo(Component? other);

        public static bool operator <(Component? left, Component? right) =>
            left is null
                ? right is not null
                : left.CompareTo(right) < 0;

        public static bool operator <=(Component? left, Component? right) =>
            left is null ||
            left.CompareTo(right) <= 0;

        public static bool operator >(Component? left, Component? right) =>
            left is not null &&
            left.CompareTo(right) > 0;

        public static bool operator >=(Component? left, Component? right) =>
            left is null
                ? right is null
                : left.CompareTo(right) >= 0;

        #endregion

        #region Equality

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Component component &&
            Equals(component);

        /// <inheritdoc/>
        public bool Equals(Component? other) => CompareTo(other) == 0;

        /// <inheritdoc/>
        public override int GetHashCode() =>
            throw new InvalidOperationException("Should be implemented in a derived class.");

        public static bool operator ==(Component? left, Component? right) =>
            left is null
                ? right is null
                : left.Equals(right);

        public static bool operator !=(Component? left, Component? right) => !(left == right);

        #endregion

        /// <inheritdoc/>
        public override string? ToString() => Value?.ToString();
    }

    #endregion
}
