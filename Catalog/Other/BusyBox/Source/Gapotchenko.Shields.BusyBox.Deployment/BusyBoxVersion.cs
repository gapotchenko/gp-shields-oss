// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.BusyBox.Deployment;

/// <summary>
/// Provides operations for BusyBox version.
/// </summary>
public static class BusyBoxVersion
{
    /// <summary>
    /// Checks whether the specified version is a canonical version of BusyBox.
    /// </summary>
    /// <remarks>
    /// For example, version <c>1.37</c> is canonical while <c>1.37.0</c> is not.
    /// The <see langword="null"/> version is canonical by convention.
    /// </remarks>
    /// <param name="version">The version.</param>
    /// <returns>
    /// <see langword="true"/> when the version is a canonical version of BusyBox;
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
    /// Canonicalizes the version of BusyBox.
    /// </summary>
    /// <remarks>
    /// For example, version <c>1.37.0</c> is canonicalized to <c>1.37</c>.
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
    /// Gets display name for the specified BusyBox version.
    /// </summary>
    /// <remarks>
    /// For example, display name is "1.37.0" for version <c>1.37.0</c>.
    /// </remarks>
    /// <param name="version">The version.</param>
    /// <returns>The display name.</returns>
    [return: NotNullIfNotNull(nameof(version))]
    public static string? GetDisplayName(Version? version) => version?.ToString();

    /// <summary>
    /// Naturalizes the specified interval of BusyBox versions.
    /// </summary>
    /// <param name="versions">The interval of BusyBox versions to naturalize.</param>
    /// <returns>The naturalized interval of BusyBox versions.</returns>
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
