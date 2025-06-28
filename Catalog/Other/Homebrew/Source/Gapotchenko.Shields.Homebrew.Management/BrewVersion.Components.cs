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
    /// Gets the major version component.
    /// </summary>
    public BrewVersionComponent Major => GetComponent(0);

    /// <summary>
    /// Gets the minor version component.
    /// </summary>
    public BrewVersionComponent Minor => GetComponent(1);

    /// <summary>
    /// Gets the patch version component.
    /// </summary>
    public BrewVersionComponent Patch => GetComponent(2);

    BrewVersionComponent GetComponent(int i) => GetComponent(Components, i);

    static BrewVersionComponent GetComponent(IReadOnlyList<BrewVersionComponent> components, int i) =>
        i < components.Count ? components[i] : BrewVersionComponent.Null.Instance;

    /// <summary>
    /// Gets the version components.
    /// </summary>
    public IReadOnlyList<BrewVersionComponent> Components => field ??= [.. ParseComponents(m_Value)];

    static IEnumerable<BrewVersionComponent> ParseComponents(string s)
    {
        foreach (Match match in Regex.Matches(s, RegexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            yield return BrewVersionComponent.From(match.Value);
    }

    static readonly string RegexPattern = string.Join("|",
        BrewVersionComponent.Alpha.Regex.ToString(),
        BrewVersionComponent.Beta.Regex.ToString(),
        BrewVersionComponent.Prerelease.Regex.ToString(),
        BrewVersionComponent.ReleaseCandidate.Regex.ToString(),
        BrewVersionComponent.Patch.Regex.ToString(),
        BrewVersionComponent.Post.Regex.ToString(),
        BrewVersionComponent.Numeric.Regex.ToString(),
        BrewVersionComponent.Text.Regex.ToString());
}
