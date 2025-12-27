# Gapotchenko.Shields.Canonical.Snap.Resolution

[![License](https://img.shields.io/badge/license-MPL2.0-green.svg)](../../../../../LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Gapotchenko.Shields.Canonical.Snap.Resolution.svg)](https://www.nuget.org/packages/Gapotchenko.Shields.Canonical.Snap.Resolution)

The resolution module of the [Gapotchenko Shield for Canonical Snap](https://github.com/gapotchenko/gp-shields-oss/tree/main/Catalog/Canonical/Snap#readme).
This module provides package resolution services for Canonical Snap, allowing you to resolve file paths to their actual locations within Snap packages.

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
3. **Package Identification**: It finds the associated Snap package
4. **Manifest Lookup**: It reads the `snap.yaml` package manifest file to find the actual file path within the package

## Platform Support

- **Linux**: Full support. Resolves paths for Snap packages installed on Linux systems.
- **macOS**: Placeholder implementation (Snap can be installed on macOS but cannot be realistically used yet).
- **Windows**: Snap cannot be installed on Windows. The resolution operation returns the original path unchanged.

## API Reference

### `SnapResolution`

Main entry point for package resolution operations.

- `GetRealFilePath(string?)` - Gets the real path of a file managed by Canonical Snap package manager. If the file is not managed by Snap, returns the original file path.

## Distribution

`Gapotchenko.Shields.Canonical.Snap.Resolution` module is available as a [NuGet package](https://nuget.org/packages/Gapotchenko.Shields.Canonical.Snap.Resolution):

```
dotnet package add Gapotchenko.Shields.Canonical.Snap.Resolution
```

## Related Modules

- [Deployment](../Gapotchenko.Shields.Canonical.Snap.Deployment#readme) - Deployment discovery services (this module depends on it)
- [Management](../Gapotchenko.Shields.Canonical.Snap.Management#readme) - Package management operations (this module depends on it)
