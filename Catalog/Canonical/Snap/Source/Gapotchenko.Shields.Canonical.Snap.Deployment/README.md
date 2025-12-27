# Gapotchenko.Shields.Canonical.Snap.Deployment

[![License](https://img.shields.io/badge/license-MPL2.0-green.svg)](../../../../../LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Gapotchenko.Shields.Canonical.Snap.Deployment.svg)](https://www.nuget.org/packages/Gapotchenko.Shields.Canonical.Snap.Deployment)

The deployment module of the [Gapotchenko Shield for Canonical Snap](https://github.com/gapotchenko/gp-shields-oss/tree/main/Catalog/Canonical/Snap#readme).
This module provides deployment discovery services for identifying installed instances of Canonical Snap on a system.

## Overview

The deployment module enables you to discover and enumerate Canonical Snap installations on your system. It provides a unified interface for locating Snap setup instances across different platforms, abstracting away the platform-specific details of where Snap is installed.

## Features

- **Discovery**: Automatically discovers installed Snap instances on supported platforms
- **Cross-platform**: Works on Linux, macOS, and Windows (with platform-specific implementations)
- **Path Resolution**: Provides utilities to resolve paths relative to Snap installation directories
- **Attribute Tracking**: Tracks how instances were discovered (e.g., via PATH environment variable)

## Usage

### Enumerating Setup Instances

The primary way to use this module is through the `SnapDeployment.EnumerateSetupInstances()` method:

```csharp
using Gapotchenko.Shields.Canonical.Snap.Deployment;

// Enumerate all Snap setup instances
foreach (var instance in SnapDeployment.EnumerateSetupInstances())
{
    Console.WriteLine($"Installation Path: {instance.InstallationPath}");
    Console.WriteLine($"Product Path: {instance.ResolvePath(instance.ProductPath)}");
    Console.WriteLine($"Attributes: {instance.Attributes}");
}

// With discovery options
var instances = SnapDeployment.EnumerateSetupInstances(
    SnapDiscoveryOptions.NoSort
);
```

### Working with Setup Instances

Each setup instance provides information about the Snap installation:

```csharp
var instance = SnapDeployment.EnumerateSetupInstances().FirstOrDefault();

if (instance != null)
{
    // Get the root installation path (e.g., "/snap")
    string installationPath = instance.InstallationPath;
    
    // Get the full path to the snap executable (e.g., "/usr/bin/snap")
    string fullProductPath = instance.ResolvePath(instance.ProductPath);

    // Resolve relative paths within the installation
    string specialPath = instance.ResolvePath("");
    
    // Check how the instance was discovered
    bool discoveredViaPath = (instance.Attributes & SnapSetupInstanceAttributes.Path) != 0;
}
```

## Platform Support

- **Linux**: Full support. Discovers Snap installations at `/snap` and locates the `snap` executable at `/usr/bin/snap` or in the PATH environment variable.
- **macOS**: Placeholder implementation (Snap can be installed on macOS but cannot be realistically used yet).
- **Windows**: Snap cannot be installed on Windows. The discovery operation does not fail, but returns zero setup instances.

## API Reference

### `SnapDeployment`

Main entry point for deployment discovery operations.

- `EnumerateSetupInstances(SnapDiscoveryOptions)` - Enumerates all discovered Snap setup instances

### `ISnapSetupInstance`

Interface representing a Snap setup instance.

- `InstallationPath` - Gets the root installation path (e.g., "/snap")
- `ProductPath` - Gets the relative path to the main product executable (e.g., "snap")
- `ResolvePath(string?)` - Resolves a relative path to the full path within the instance
- `Attributes` - Gets the instance attributes

### `SnapDiscoveryOptions`

Flags for controlling discovery behavior.

- `None` - Default options
- `NoSort` - Do not sort setup instances by version and edition

### `SnapSetupInstanceAttributes`

Attributes describing how an instance was discovered.

- `None` - No special attributes
- `Path` - Instance was discovered via PATH environment variable

## Installation

`Gapotchenko.Shields.Canonical.Snap.Deployment` module is available as a [NuGet package](https://nuget.org/packages/Gapotchenko.Shields.Canonical.Snap.Deployment):

```
dotnet package add Gapotchenko.Shields.Canonical.Snap.Deployment
```

## Related Modules

- [Management](../Gapotchenko.Shields.Canonical.Snap.Management#readme) - Package management operations (depends on this module)
- [Resolution](../Gapotchenko.Shields.Canonical.Snap.Resolution#readme) - Package resolution services (depends on this module)
