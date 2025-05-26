// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

namespace Gapotchenko.Shields.Java.Deployment;

/// <summary>
/// Flags for Java deployment discovery operations.
/// </summary>
[Flags]
public enum JavaDiscoveryOptions
{
    /// <summary>
    /// No options are specified. This is the default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Disallows environment deduction through the <c>JAVA_HOME</c> variable.
    /// When this option is specified,
    /// the discovery algorithm won't try to use the deduction heuristics based on the process environment variables.
    /// </summary>
    NoEnvironment = 1 << 0,
}
