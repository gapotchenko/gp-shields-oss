# Gapotchenko.Shields.Canonical.Snap.Management

The management module of the [Gapotchenko Shield for Canonical Snap](https://github.com/gapotchenko/gp-shields-oss/tree/main/Catalog/Canonical/Snap). This module provides package management operations for Canonical Snap, allowing you to enumerate installed packages and access their properties.

## Overview

The management module enables you to work with installed Snap packages on your system.
It provides functionality to enumerate packages, query their paths, and access package directories.

## Features

- **Package Enumeration**: Enumerate all installed Snap packages or filter by package identifier
- **Revision Support**: Work with specific package revisions or current revisions
- **Path Resolution**: Get directory paths for installed packages
- **Type-Safe API**: Uses `SnapPackageName` record struct for type-safe package identification

## Usage

### Creating a Manager

First, you need to obtain a setup instance from the Deployment module and create a manager:

```csharp
using Gapotchenko.Shields.Canonical.Snap.Deployment;
using Gapotchenko.Shields.Canonical.Snap.Management;

// Get a setup instance
var setupInstance = SnapDeployment.EnumerateSetupInstances().FirstOrDefault();

if (setupInstance != null)
{
    // Create a manager for the setup instance
    var manager = SnapManagement.CreateManager(setupInstance);
    
    // Use the manager to work with packages
    // ...
}
```

### Enumerating Packages

Enumerate all installed packages:

```csharp
var manager = SnapManagement.CreateManager(setupInstance);

// Enumerate all packages (all revisions)
foreach (var package in manager.EnumeratePackages())
{
    Console.WriteLine($"Package: {package.Id}, Revision: {package.Revision}");
}

// Enumerate only current revisions
foreach (var package in manager.EnumeratePackages(SnapPackageEnumerationOptions.Current))
{
    Console.WriteLine($"Current: {package}");
}
```

### Filtering by Package Identifier

Enumerate packages for a specific identifier:

```csharp
// Enumerate all revisions of a specific package
foreach (var package in manager.EnumeratePackages("code"))
{
    Console.WriteLine($"Visual Studio Code revision: {package.Revision}");
}

// Get only the current revision
var currentCode = manager.EnumeratePackages(
    "code",
    SnapPackageEnumerationOptions.Current
).FirstOrDefault();
```

### Getting Package Paths

Retrieve directory paths for installed packages:

```csharp
var package = manager
    .EnumeratePackages("code", SnapPackageEnumerationOptions.Current)
    .FirstOrDefault();

if (package == null)
    throw new Exception("Snap package for Visual Studio Code not found.");

string packagePath = manager.GetPackagePath(package);
Console.WriteLine($"Package directory: {packagePath}");
```

### Working with Package Names

Create and work with `SnapPackageName`:

```csharp
// Create a package name
var package = new SnapPackageName("code", 123);

// Access properties
string packageId = package.Id;        // "code"
int revision = package.Revision;      // 123

// String representation
string displayName = package.ToString(); // "code/123"
```

## Platform Support

- **Linux**: Full support. Enumerates packages from the `/snap` directory structure.
- **macOS**: Placeholder implementation (Snap can be installed on macOS but cannot be realistically used yet).
- **Windows**: Snap cannot be installed on Windows. The management operations will work but return no packages.

## API Reference

### `SnapManagement`

Main entry point for creating package managers.

- `CreateManager(ISnapSetupInstance)` - Creates a manager for the specified setup instance

### `ISnapManager`

Interface providing package management operations.

- `EnumeratePackages(SnapPackageEnumerationOptions)` - Enumerates all installed packages
- `EnumeratePackages(string?, SnapPackageEnumerationOptions)` - Enumerates packages with optional identifier filter
- `GetPackagePath(SnapPackageName)` - Gets the directory path for a package (throws if not found)
- `TryGetPackagePath(SnapPackageName)` - Tries to get the directory path for a package (returns null if not found)
- `Setup` - Gets the associated setup instance

### `SnapPackageName`

Record struct representing a Snap package name with identifier and revision.

- `Id` - Gets the package identifier (e.g., "code")
- `Revision` - Gets the package revision number (e.g., 123)
- `ToString()` - Returns a string representation in the format "id/revision"

### `SnapPackageEnumerationOptions`

Flags for controlling package enumeration behavior.

- `None` - Default options (enumerates all revisions)
- `Current` - Enumerate only current package revisions

## Related Modules

- [Deployment](../Gapotchenko.Shields.Canonical.Snap.Deployment) - Deployment discovery services (this module depends on it)
- [Resolution](../Gapotchenko.Shields.Canonical.Snap.Resolution) - Package resolution services (depends on this module)

