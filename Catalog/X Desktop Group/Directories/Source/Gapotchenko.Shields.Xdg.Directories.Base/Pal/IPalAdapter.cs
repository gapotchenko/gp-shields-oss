// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.Base.Pal;

interface IPalAdapter
{
    /// <summary>
    /// Gets a default value of <c>XDG_DATA_HOME</c> variable.
    /// </summary>
    string GetDataHome();

    /// <summary>
    /// Gets a default value of <c>XDG_DATA_DIRS</c> variable.
    /// </summary>
    string GetDataDirectories();

    /// <summary>
    /// Gets a default value of <c>XDG_CONFIG_HOME</c> variable.
    /// </summary>
    string GetConfigurationHome();

    /// <summary>
    /// Gets a default value of <c>XDG_CONFIG_DIRS</c> variable.
    /// </summary>
    string GetConfigurationDirectories();

    /// <summary>
    /// Gets a default value of <c>XDG_STATE_HOME</c> variable.
    /// </summary>
    string GetStateHome();

    /// <summary>
    /// Gets a default value of <c>XDG_CACHE_HOME</c> variable.
    /// </summary>
    string GetCacheHome();

    /// <summary>
    /// Gets a default value of <c>XDG_RUNTIME_DIR</c> variable.
    /// </summary>
    string GetRuntimeDirectory();
}
