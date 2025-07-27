// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Git.Deployment;

/// <summary>
/// Provides operations for Git version.
/// </summary>
public static class GitVersion
{
    /// <summary>
    /// Checks whether the specified version is a canonical version of Git.
    /// </summary>
    /// <remarks>
    /// For example, version <c>2.50</c> is canonical while <c>2.50.1</c> is not.
    /// The <see langword="null"/> version is canonical by convention.
    /// </remarks>
    /// <param name="version">The version.</param>
    /// <returns>
    /// <see langword="true"/> when the version is a canonical version of Git;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsCanonical([NotNullWhen(false)] Version? version)
    {
        if (version == null)
        {
            // Null version is canonical by convention.
            return true;
        }
        else if (version.Build == -1)
        {
            // X.Y versions are canonical.
            return true;
        }
        else
        {
            // X.Y.* versions are not canonical.
            return false;
        }
    }

    /// <summary>
    /// Canonicalizes the version of Git.
    /// </summary>
    /// <remarks>
    /// For example, version <c>2.50.1</c> is canonicalized to <c>2.50</c>.
    /// </remarks>
    /// <param name="version">The version to canonicalize or <see langword="null"/>.</param>
    /// <returns>The canonicalized version, or <see langword="null"/> when the specified version is <see langword="null"/>.</returns>
    [return: NotNullIfNotNull(nameof(version))]
    public static Version? Canonicalize(Version? version)
    {
        if (IsCanonical(version))
            return version;
        else
            return new Version(version.Major, version.Minor);
    }

    /// <summary>
    /// Gets display name for the specified Git version.
    /// </summary>
    /// <remarks>
    /// For example, display name is "2.50.1" for version <c>2.50.1</c>.
    /// </remarks>
    /// <param name="version">The version.</param>
    /// <returns>The display name.</returns>
    [return: NotNullIfNotNull(nameof(version))]
    public static string? GetDisplayName(Version? version) => version?.ToString();

    /// <summary>
    /// Naturalizes the specified interval of Git versions.
    /// </summary>
    /// <param name="versions">The interval of Git versions to naturalize.</param>
    /// <returns>The naturalized interval of Git versions.</returns>
    public static ValueInterval<Version> NaturalizeInterval(in ValueInterval<Version> versions)
    {
        if (versions.To.Kind == IntervalBoundaryKind.Inclusive)
        {
            var to = versions.To.Value;
            if (IsCanonical(to))
            {
                // Widen a canonical interval like [...,X.Y] to [...,X.Y+1) to handle non-canonical X.Y.* versions in between.
                return versions with
                {
                    To = IntervalBoundary.Exclusive(new Version(to.Major, to.Minor + 1))
                };
            }
            else if (to.Revision == -1)
            {
                // Right interval boundary is a fractional version like X.Y.Z (as opposed to non-fractional X.Y).
                //
                // [X.Y.Z,X.Y.Z] would not constitute a good interval as the real product versions may have four elements like X.Y.Z.*.
                //
                // That's why the interval is widened up to cover the version revisions like X.Y.Z.*:
                //
                //     [X.Y.Z,X.Y.Z] -> [X.Y.Z,X.Y.Z+1)
                //
                // making X.Y.Z.* Є [X.Y.Z,X.Y.Z+1).

                return versions with
                {
                    To = IntervalBoundary.Exclusive(new Version(to.Major, to.Minor, to.Build + 1))
                };
            }
        }

        return versions;
    }
}
