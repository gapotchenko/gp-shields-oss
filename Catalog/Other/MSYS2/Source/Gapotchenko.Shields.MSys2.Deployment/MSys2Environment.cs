// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

sealed class MSys2Environment(
    IMSys2SetupInstance setupInstance,
    string installationPath,
    string productPath,
    string name) :
    IMSys2Environment,
    IFormattable
{
    public string Name => name;

    public string InstallationPath => installationPath;

    public string ProductPath => productPath;

    public IMSys2SetupInstance SetupInstance => setupInstance;

    public Architecture Architecture =>
        name switch
        {
            // https://www.msys2.org/docs/environments
            "MSYS" => Architecture.X64,
            "UCRT64" => Architecture.X64,
            "CLANG64" => Architecture.X64,
            "CLANGARM64" => Architecture.Arm64,
            "MINGW64" => Architecture.X64,
            // Legacy environments.
            "MINGW32" => Architecture.X86,
            "CLANG32" => Architecture.X86,
            _ => throw new MSys2DeploymentException("Cannot determine the architecture of a MSYS2 environment.")
        };

    #region Formatting

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format);

    public string ToString(string? format) =>
        format switch
        {
            "G" or null => ToString()!,
            "D" => Name,
            _ => throw new FormatException("Format specifier was invalid.")
        };

    #endregion
}
