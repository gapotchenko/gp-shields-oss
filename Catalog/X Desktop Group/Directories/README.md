# Gapotchenko Shield for XDG Directories

Implements XDG Base Directory and XDG User Directories specifications.

## Summary

freedesktop.org, formerly X Desktop Group (XDG), is a project to work on
interoperability and shared base technology for desktop environments provided
by various operating systems.

XDG directory specifications define where application files should be looked for
relative to one or more predefined base directories.

## XDG Base Directories

XDG Base Directories are regulated by [XDG Base Directory Specification](https://specifications.freedesktop.org/basedir-spec/basedir-spec-latest.html).

This functionality is provided by `Gapotchenko.Shields.Xdg.Directories.Base` module.

## XDG User Directories

XDG User Directories are regulated by `xdg-user-dirs` command-line utility.
The informal overview is available at https://wiki.archlinux.org/title/XDG_user_directories.

This functionality is provided by `Gapotchenko.Shields.Xdg.Directories.User` module.

## Notable Implementations

  - https://github.com/adrg/xdg (golang)
