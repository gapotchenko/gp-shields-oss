# Gapotchenko Shield for Canonical Snap

A [technology shield](../../..#overview) that provides ready-to-use modules for working with [Canonical Snap](https://snapcraft.io/) technology.

Snap is a software packaging and deployment system developed by Canonical for operating systems that use the Linux kernel and the systemd init system. The packages, called snaps, and the tool for using them, `snapd`, work across a range of Linux distributions and allow upstream software developers to distribute their applications directly to users. Snaps are self-contained applications running on a host system.

## Modules

The shield consists of the following modules:

- **[Deployment](Source/Gapotchenko.Shields.Canonical.Snap.Deployment#readme)** - Identifies installed instances of Canonical Snap on a system
- **[Management](Source/Gapotchenko.Shields.Canonical.Snap.Management#readme)** - Enumerates installed Snap packages and accesses their properties
- **[Resolution](Source/Gapotchenko.Shields.Canonical.Snap.Resolution#readme)** - Resolves file paths to their actual locations within Snap packages

## Quick Start

### Discovering Snap Installations

```csharp
using Gapotchenko.Shields.Canonical.Snap.Deployment;

// Enumerate all Snap setup instances
foreach (var instance in SnapDeployment.EnumerateSetupInstances())
{
    Console.WriteLine($"Installation Path: {instance.InstallationPath}");
}
```

### Working with Installed Packages

```csharp
using Gapotchenko.Shields.Canonical.Snap.Deployment;
using Gapotchenko.Shields.Canonical.Snap.Management;

// Get a setup instance and create a manager
var setupInstance = SnapDeployment.EnumerateSetupInstances().FirstOrDefault();
if (setupInstance != null)
{
    var manager = SnapManagement.CreateManager(setupInstance);
    
    // Enumerate all installed packages
    foreach (var package in manager.EnumeratePackages(SnapPackageEnumerationOptions.Current))
    {
        Console.WriteLine($"Package: {package.Id}, Revision: {package.Revision}");
    }
}
```

### Resolving Snap-Managed File Paths

```csharp
using Gapotchenko.Shields.Canonical.Snap.Resolution;

// Resolve a file path that might be managed by Snap
string filePath = "/usr/bin/dotnet";
string realPath = SnapResolution.GetRealFilePath(filePath);

// If dotnet is installed via Snap, realPath points to the actual location
// within the Snap package directory
```

## Platform Support

- **Linux**: Full support for all modules
- **macOS**: Placeholder implementation (Snap can be installed but cannot be realistically used yet)
- **Windows**: Snap cannot be installed on Windows; operations return empty results or original paths
