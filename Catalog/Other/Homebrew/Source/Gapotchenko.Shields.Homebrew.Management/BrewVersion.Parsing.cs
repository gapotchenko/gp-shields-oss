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
    public static BrewVersion? Parse(string? input) =>
        input is null
            ? null
            : new(ParseCore(input));

    /// <summary>
    /// Tries to convert the string representation of a Homebrew package version to an equivalent <see cref="BrewVersion"/> object,
    /// and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="input">A string that contains a Homebrew package version to convert.</param>
    /// <param name="result">
    /// When this method returns, contains the <see cref="BrewVersion"/> equivalent of the Homebrew package version string specified in the input, if the conversion succeeded;
    /// or <see langword="null"/> if the conversion failed.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="input"/> parameter was converted successfully;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out BrewVersion? result) =>
        (result = TryParse(input)) is not null;

    /// <summary>
    /// Tries to convert the string representation of a Homebrew package version to an equivalent <see cref="BrewVersion"/> object.
    /// </summary>
    /// <param name="input">A string that contains a Homebrew package version to convert.</param>
    /// <returns>
    /// An object that is equivalent to the Homebrew package version specified in the <paramref name="input"/> parameter if conversion was successful;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    [return: NotNullIfNotNull(nameof(input))]
    public static BrewVersion? TryParse(string? input) =>
        input is null
            ? null
            : Create(ParseCore(input, false));

    static Model ParseCore(string input) =>
        ParseCore(input, true) ??
        throw new FormatException("The version string has an invalid format.");

    static Model? ParseCore(string input, bool throwOnError)
    {
        if (input is [])
        {
            if (throwOnError)
                throw new FormatException("The version string is empty.");
            return null;
        }

        var components = new List<BrewVersionComponent>();
        foreach (Match match in BrewVersionComponent.Regex.Matches(input))
        {
            string value = match.Value;

            BrewVersionComponent? component;
            if (throwOnError)
            {
                component = BrewVersionComponent.Parse(value);
            }
            else
            {
                component = BrewVersionComponent.TryParse(value);
                if (component is null)
                    return null;
            }

            components.Add(component);
        }

        return new Model
        {
            Version = input,
            Components = [.. components]
        };
    }
}
