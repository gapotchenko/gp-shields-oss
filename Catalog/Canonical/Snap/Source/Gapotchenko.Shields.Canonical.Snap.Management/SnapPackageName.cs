// Gapotchenko.Shields.Canonical.Snap.Management
// Copyright © Gapotchenko
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
    public SnapPackageName()
    {
        m_Id = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnapPackageName"/> record.
    /// </summary>
    /// <param name="id">The package identifier.</param>
    /// <param name="revision">The package revision.</param>
    public SnapPackageName(string id, int revision)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));
        ValidateId(id);

        m_Id = id;
        Revision = revision;
    }

    /// <summary>
    /// Gets or initializes the package identifier.
    /// </summary>
    public string Id
    {
        get => m_Id;
        init
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            ValidateId(value);

            m_Id = value;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string m_Id;

    /// <summary>
    /// Gets or initializes the package revision.
    /// </summary>
    public int Revision { get; init; }

    /// <inheritdoc/>
    public override string ToString() => Invariant($"{m_Id}/{Revision}");

    [StackTraceHidden]
    internal static void ValidateId(string id, [CallerArgumentExpression(nameof(id))] string? parameterName = null)
    {
        int j = id.AsSpan().IndexOfAny(new[] { '?', '*', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
        if (j != -1)
        {
            throw new ArgumentException(
                string.Format(
                    "Snap package identifier contains a prohibited symbol '{0}'.",
                    id[j]),
                parameterName);
        }
    }
}
