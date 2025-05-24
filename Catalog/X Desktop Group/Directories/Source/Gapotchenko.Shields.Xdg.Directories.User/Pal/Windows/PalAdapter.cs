// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Pal.Windows;

#if NET
[SupportedOSPlatform("windows")]
#endif
sealed class PalAdapter : IPalAdapter
{
    PalAdapter()
    {
    }

    public static PalAdapter Instance { get; } = new();

    public string GetDesktopDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

    public string GetDownloadsDirectory() => GetKnownShellFolderPath(new("374DE290-123F-4565-9164-39C4925E467B"));

    public string GetDocumentsDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public string GetMusicDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    public string GetPicturesDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

    public string GetVideosDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

    public string GetTemplatesDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.Templates);

    public string GetPublicDirectory() => GetKnownShellFolderPath(new("DFDF76A2-C82A-4D63-906A-5644AC457385"));

    static string GetKnownShellFolderPath(in Guid guid) => NativeMethods.SHGetKnownFolderPath(guid, 0, IntPtr.Zero);
}
