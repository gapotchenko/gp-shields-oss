# Gapotchenko.Shields.Canonical.Snap.Resolution

The resolution module of the [Gapotchenko Shield for Canonical Snap](https://github.com/gapotchenko/gp-shields-oss/tree/main/Catalog/Canonical/Snap). This module provides package resolution services for Canonical Snap, allowing you to resolve file paths to their actual locations within Snap packages.

## Overview

The resolution module enables you to resolve file paths that are managed by Canonical Snap packages.
When Snap packages install executables, they often create symbolic links in system directories (like `/usr/bin`) that point to the actual executables within the Snap package directories.
This module helps you find the real file paths inside Snap packages, even when accessed through these symbolic links.

## Features

- **Path Resolution**: Resolves symbolic links to find actual file locations within Snap packages
- **Snap Detection**: Automatically detects if a file is managed by a Snap package
- **Transparent Fallback**: Returns the original path if the file is not managed by Snap

## Usage

### Resolving File Paths

The primary functionality is provided by the `GetRealFilePath` method:

```csharp
using Gapotchenko.Shields.Canonical.Snap.Resolution;

// Resolve a file path that might be managed by Snap
string filePath = "/usr/bin/dotnet";
string realPath = SnapResolution.GetRealFilePath(filePath);

Console.WriteLine($"Original path: {filePath}");
Console.WriteLine($"Real path: {realPath}");

// If dotnet is installed via Snap, realPath might be something like:
// "/snap/dotnet-sdk/123/usr/share/dotnet/dotnet".
// Otherwise, GetRealFilePath function returns the original path.
```

### Working with Snap-Managed Executables

When a Snap package installs an executable, it typically creates a symbolic link in a system directory.
The resolution module can trace these links to find the actual executable within the Snap package:

```csharp
// Example: Resolving a .NET SDK installed via Snap
string dotnetPath = "/usr/bin/dotnet";
string realDotnetPath = SnapResolution.GetRealFilePath(dotnetPath);

// If dotnet is installed via Snap, realDotnetPath might be something like:
// "/snap/dotnet-sdk/123/usr/share/dotnet/dotnet"
// Otherwise, it returns the original path
```

### Handling Non-Existent Files

The method handles non-existent files gracefully:

```csharp
string nonExistentPath = "/usr/bin/nonexistent";
string resolved = SnapResolution.GetRealFilePath(nonExistentPath);
// Returns the original path if the file doesn't exist
```

## How It Works

1. **Symbolic Link Resolution**: The module follows symbolic links to find the final target
2. **Snap Detection**: It checks if the resolved path points to the Snap executable (`snap`)
3. **Manifest Lookup**: It reads the `snap.yaml` manifest file to find the actual file path

## Platform Support

- **Linux**: Full support. Resolves paths for Snap packages installed on Linux systems.
- **macOS**: Placeholder implementation (Snap can be installed on macOS but cannot be realistically used yet).
- **Windows**: Snap cannot be installed on Windows. The resolution operation returns the original path unchanged.

## API Reference

### `SnapResolution`

Main entry point for package resolution operations.

- `GetRealFilePath(string?)` - Gets the real path of a file managed by Canonical Snap package manager. If the file is not managed by Snap, returns the original file path.

## Related Modules

- [Deployment](../Gapotchenko.Shields.Canonical.Snap.Deployment) - Deployment discovery services (this module depends on it)
- [Management](../Gapotchenko.Shields.Canonical.Snap.Management) - Package management operations (this module depends on it)

