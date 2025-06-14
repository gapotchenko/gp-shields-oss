// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

/// <summary>
/// Represents a Microsoft WSL deployment error.
/// </summary>
public class WslDeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WslDeploymentException"/> class.
    /// </summary>
    public WslDeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WslDeploymentException"/> class with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public WslDeploymentException(string? message) :
        base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WslDeploymentException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public WslDeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
