// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using System.Runtime.CompilerServices;

namespace Gapotchenko.Shields.Canonical.Snap.Management;

/// <summary>
/// Represents the name of a snap package.
/// </summary>
public readonly record struct SnapPackageName
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnapPackageName"/> record.
    /// </summary>
    /// <param name="id">The package identifier.</param>
    /// <param name="revision">The package revision.</param>
    public SnapPackageName(string id, int revision)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ValidateId(id);
        ValidateRevision(revision);

        m_Id = id;
        m_Revision = revision;
    }

    /// <summary>
    /// Gets or initializes the package identifier.
    /// </summary>
    public string Id
    {
        get => m_Id;
        init
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            ValidateId(value);

            m_Id = value;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string m_Id;

    /// <summary>
    /// Gets or initializes the package revision.
    /// </summary>
    public int Revision
    {
        get => m_Revision;
        init
        {
            ValidateRevision(value);
            m_Revision = value;
        }
    }

    readonly int m_Revision;

    /// <inheritdoc/>
    public override string ToString() => Invariant($"{m_Id}/{Revision}");

    [StackTraceHidden]
    internal static void ValidateId(
        ReadOnlySpan<char> id,
        [CallerArgumentExpression(nameof(id))] string? paramName = null)
    {
        int j = id.IndexOfAny(['?', '*', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar]);
        if (j != -1)
        {
            throw new ArgumentException(
                string.Format(
                    "The value contains a prohibited symbol '{0}'.",
                    id[j]),
                paramName);
        }
    }

    [StackTraceHidden]
    static void ValidateRevision(
        int revision,
        [CallerArgumentExpression(nameof(revision))] string? paramName = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(revision, paramName);
    }
}
