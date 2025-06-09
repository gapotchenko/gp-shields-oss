// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.MSys2.Deployment;

namespace Gapotchenko.Shields.MSys2.Harness;

class Program
{
    static void Main()
    {
        try
        {
            DumpInstalledSetupInstances();
            DumpPortableSetupInstances();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void DumpInstalledSetupInstances()
    {
        Console.WriteLine("*** Installed MSys2 Setup Instances ***");
        Console.WriteLine();

        foreach (var (i, instance) in
            Enumerable.Range(1, int.MaxValue)
            .Zip(MSys2Deployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void DumpPortableSetupInstances()
    {
        Console.WriteLine("*** Portable MSys2 Setup Instances ***");
        Console.WriteLine();

        string[] paths = [@"C:\msys64"];

        foreach (var (i, path) in Enumerable.Range(1, int.MaxValue).Zip(paths))
        {
            Console.WriteLine("#{0} at '{1}'", i, path);

            var instance = MSys2SetupInstance.TryOpen(path);
            if (instance is null)
                Console.WriteLine("MSYS2 setup instance is not found.");
            else
                PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(IMSys2SetupInstance instance)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "MSYS2.Product");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Display name: {0}", instance.DisplayName);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));
    }
}
