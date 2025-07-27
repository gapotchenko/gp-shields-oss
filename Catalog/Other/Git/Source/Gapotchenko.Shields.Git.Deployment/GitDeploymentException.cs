// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Git.Deployment;

/// <summary>
/// Represents a Git deployment error.
/// </summary>
public class GitDeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitDeploymentException"/> class.
    /// </summary>
    public GitDeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitDeploymentException"/> class
    /// with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public GitDeploymentException(string? message) :
        base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitDeploymentException"/> class
    /// with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public GitDeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
