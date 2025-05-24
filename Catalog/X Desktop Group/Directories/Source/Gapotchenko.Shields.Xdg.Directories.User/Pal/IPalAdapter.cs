// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Pal;

interface IPalAdapter
{
    /// <summary>
    /// Gets a default value of <c>XDG_DESKTOP_DIR</c> variable.
    /// </summary>
    string GetDesktopDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_DOWNLOAD_DIR</c> variable.
    /// </summary>
    string GetDownloadsDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_DOCUMENTS_DIR</c> variable.
    /// </summary>
    string GetDocumentsDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_MUSIC_DIR</c> variable.
    /// </summary>
    string GetMusicDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_PICTURES_DIR</c> variable.
    /// </summary>
    string GetPicturesDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_VIDEOS_DIR</c> variable.
    /// </summary>
    string GetVideosDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_TEMPLATES_DIR</c> variable.
    /// </summary>
    string GetTemplatesDirectory();

    /// <summary>
    /// Gets a default value of <c>XDG_PUBLICSHARE_DIR</c> variable.
    /// </summary>
    string GetPublicDirectory();
}
