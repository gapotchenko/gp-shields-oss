// Gapotchenko.Shields.Cygwin
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Cygwin.Deployment;

/// <summary>
/// Represents a Cygwin deployment error.
/// </summary>
public class CygwinDeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CygwinDeploymentException"/> class.
    /// </summary>
    public CygwinDeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CygwinDeploymentException"/> class with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public CygwinDeploymentException(string? message) :
        base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CygwinDeploymentException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public CygwinDeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
