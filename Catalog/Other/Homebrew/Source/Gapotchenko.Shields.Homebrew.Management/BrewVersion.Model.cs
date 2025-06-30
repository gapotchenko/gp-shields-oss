// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;

namespace Gapotchenko.Shields.Homebrew.Management;

partial record BrewVersion
{
    /// <summary>
    /// The object model of a Homebrew package version.
    /// </summary>
    readonly struct Model
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Model"/> structure with the specified version string.
        /// </summary>
        /// <param name="version">A string containing the Homebrew package version.</param>
        /// <returns>An object model of the specified target Homebrew package version.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="version"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="version"/> is empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="version"/> has an invalid format.</exception>
        public static Model Create(string version)
        {
            ArgumentException.ThrowIfNullOrEmpty(version);

            try
            {
                return ParseCore(version);
            }
            catch (FormatException e)
            {
                throw new ArgumentException(e.Message, nameof(version), e.InnerException);
            }
        }

        public required string Version { get; init; }
        public required IReadOnlyList<BrewVersionComponent> Components { get; init; }
    }

    [return: NotNullIfNotNull(nameof(model))]
    static BrewVersion? Create(in Model? model) => model is null ? null : new(model.Value);

    BrewVersion(in Model model)
    {
        m_Version = model.Version;
        Components = model.Components;
    }
}
