// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using System.Text.RegularExpressions;

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion
{
    /// <summary>
    /// Converts the string representation of a Homebrew package version to an equivalent <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="input">A string that contains a Homebrew package version to convert.</param>
    /// <returns>
    /// An object that is equivalent to the Homebrew package version string specified in the <paramref name="input"/> parameter,
    /// or <see langword="null"/> if <paramref name="input"/> parameter is <see langword="null"/>.
    /// </returns>
    /// <exception cref="FormatException"><paramref name="input"/> string has an invalid format.</exception>
    [return: NotNullIfNotNull(nameof(input))]
    public static BrewVersion? Parse(string? input)
    {
        if (input is null)
            return null;

        try
        {
            return new BrewVersion(input);
        }
        catch (ArgumentException)
        {
            throw new FormatException("Homebrew version string has an invalid format.");
        }
    }

    static IEnumerable<BrewVersionComponent> ParseComponents(string s)
    {
        foreach (Match match in BrewVersionComponent.Regex.Matches(s))
            yield return BrewVersionComponent.Create(match.Value);
    }
}
