// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Homebrew.Deployment;

/// <summary>
/// Provides operations for Homebrew version.
/// </summary>
public static class BrewVersion
{
    /// <summary>
    /// Checks whether the specified version is a canonical version of Homebrew.
    /// </summary>
    /// <remarks>
    /// For example, version <c>4.5.7</c> is canonical while <c>4.5.7.0</c> is not.
    /// The <see langword="null"/> version is canonical by convention.
    /// </remarks>
    /// <param name="version">The version.</param>
    /// <returns>
    /// <see langword="true"/> when the version is a canonical version of Homebrew;
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
            // X.Y versions are not canonical.
            return false;
        }
        else if (version.Revision != -1)
        {
            // X.Y.Z.* versions are not canonical.
            return false;
        }
        else
        {
            // X.Y.Z versions are canonical.
            return true;
        }
    }

    /// <summary>
    /// Canonicalizes the version of Homebrew.
    /// </summary>
    /// <remarks>
    /// For example, version <c>4.5.7.0</c> is canonicalized to <c>4.5.7</c>.
    /// </remarks>
    /// <param name="version">The version to canonicalize or <see langword="null"/>.</param>
    /// <returns>The canonicalized version, or <see langword="null"/> when the specified version is <see langword="null"/>.</returns>
    [return: NotNullIfNotNull(nameof(version))]
    public static Version? Canonicalize(Version? version)
    {
        if (IsCanonical(version))
            return version;
        else
            return new Version(version.Major, version.Minor, version.Build is not -1 and var build ? build : 0);
    }

    /// <summary>
    /// <para>
    /// Gets display name for the specified Homebrew version.
    /// </para>
    /// <para>
    /// For example, display name is "4.5.7" for version <c>4.5.7</c>.
    /// </para>
    /// </summary>
    /// <param name="version">The version.</param>
    /// <returns>The display name.</returns>
    [return: NotNullIfNotNull(nameof(version))]
    public static string? GetDisplayName(Version? version) => version?.ToString();

    /// <summary>
    /// Naturalizes the specified interval of Homebrew versions.
    /// </summary>
    /// <param name="versions">The interval of Homebrew versions to naturalize.</param>
    /// <returns>The naturalized interval of Homebrew versions.</returns>
    public static ValueInterval<Version> NaturalizeInterval(in ValueInterval<Version> versions)
    {
        if (versions.To.Kind == IntervalBoundaryKind.Inclusive)
        {
            var to = versions.To.Value;
            if (IsCanonical(to))
            {
                // Widen a canonical interval like [...,4.5.7] to [...,4.5.8) to handle non-canonical 4.5.7.* versions in between.
                return versions with
                {
                    To = IntervalBoundary.Exclusive(new Version(to.Major, to.Minor, to.Build + 1))
                };
            }
            else if (to.Build == -1)
            {
                // Right interval boundary is a X.Y version with two components.
                //
                // [X.Y,X.Y] would not constitute a good interval as the real product versions have three elements like X.Y.*.
                //
                // That's why the interval is widened up to cover X.Y.* versions in between:
                //
                //     [X.Y,X.Y] -> [X.Y,X.Y+1)
                //
                // making X.Y.* Є [X.Y,X.Y+1).

                return versions with
                {
                    To = IntervalBoundary.Exclusive(new Version(to.Major, to.Minor + 1))
                };
            }
        }

        return versions;
    }
}
