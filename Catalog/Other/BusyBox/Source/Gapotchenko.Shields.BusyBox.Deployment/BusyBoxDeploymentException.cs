// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.BusyBox.Deployment;

/// <summary>
/// Represents a BusyBox deployment error.
/// </summary>
public class BusyBoxDeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusyBoxDeploymentException"/> class.
    /// </summary>
    public BusyBoxDeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusyBoxDeploymentException"/> class
    /// with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public BusyBoxDeploymentException(string? message) :
        base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusyBoxDeploymentException"/> class
    /// with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public BusyBoxDeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
