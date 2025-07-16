// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Represents an MSYS2 deployment error.
/// </summary>
public class MSys2DeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MSys2DeploymentException"/> class.
    /// </summary>
    public MSys2DeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MSys2DeploymentException"/> class with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public MSys2DeploymentException(string? message) :
        base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MSys2DeploymentException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public MSys2DeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
