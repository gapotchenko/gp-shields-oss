// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Deployment;

/// <summary>
/// Represents a Homebrew deployment error.
/// </summary>
public class BrewDeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewDeploymentException"/> class.
    /// </summary>
    public BrewDeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrewDeploymentException"/> class
    /// with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public BrewDeploymentException(string? message) :
        base(message)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewDeploymentException"/> class
    /// with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public BrewDeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
