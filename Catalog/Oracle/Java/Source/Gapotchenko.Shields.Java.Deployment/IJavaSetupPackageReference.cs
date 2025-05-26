// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

namespace Gapotchenko.Shields.Java.Deployment;

/// <summary>
/// A reference to a Java package.
/// </summary>
public interface IJavaSetupPackageReference
{
    /// <summary>
    /// Gets the general package identifier.
    /// </summary>
    string Id { get; }
}
