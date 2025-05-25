// Gapotchenko.Shields.Microsoft.PowerShell
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

/// <summary>
/// Represents PowerShell deployment errors.
/// </summary>
public class PowerShellDeploymentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PowerShellDeploymentException"/> class.
    /// </summary>
    public PowerShellDeploymentException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PowerShellDeploymentException"/> class with a specified error message.
    /// </summary>
    /// <inheritdoc/>
    public PowerShellDeploymentException(string? message) :
        base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PowerShellDeploymentException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <inheritdoc/>
    public PowerShellDeploymentException(string? message, Exception? innerException) :
        base(message, innerException)
    {
    }
}
