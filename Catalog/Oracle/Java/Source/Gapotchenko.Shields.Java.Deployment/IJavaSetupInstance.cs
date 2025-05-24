// Gapotchenko.Shields.Java.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

namespace Gapotchenko.Shields.Java.Deployment;

/// <summary>
/// Provides information about the setup instance of Java.
/// </summary>
public interface IJavaSetupInstance
{
    /// <summary>
    /// Gets the path of the product home directory.
    /// </summary>
    string HomePath { get; }

    /// <summary>
    /// Gets the installed version of the product instance.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Resolves the relative path to the home path of the instance.
    /// </summary>
    /// <param name="relativePath">The relative path or <c>null</c>.</param>
    /// <returns>
    /// The full path to the relative path within the instance.
    /// If the relative path is <c>null</c>, the home path will always terminate in a backslash.
    /// </returns>
    string ResolvePath(string? relativePath);

    /// <summary>
    /// Gets a package reference to the product registered to the instance.
    /// </summary>
    IJavaSetupPackageReference Product { get; }
}
