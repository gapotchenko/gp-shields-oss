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
            ListInstalledSetupInstances();
            ListPortableSetupInstances();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void ListInstalledSetupInstances()
    {
        Console.WriteLine("*** Installed Setup Instances ***");
        Console.WriteLine();

        foreach (var (instance, i) in
            MSys2Deployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())
            .Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void ListPortableSetupInstances()
    {
        Console.WriteLine("*** Portable Setup Instances ***");
        Console.WriteLine();

        string[] paths = [@"C:\msys64"];

        foreach (var (path, i) in paths.Zip(Enumerable.Range(1, int.MaxValue)))
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

    static void PrintSetupInstance(IMSys2SetupInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "MSYS2.Product");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write(padding);
        Console.WriteLine("Display name: {0}", instance.DisplayName);

        Console.Write(padding);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);

        Console.Write(padding);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));

        Console.Write(padding);
        Console.WriteLine("Attributes: {0}", instance.Attributes);

        Console.Write(padding);
        Console.WriteLine("Environments:");
        foreach (var environment in instance.EnumerateEnvironments())
            PrintEnvironment(environment, level + 1);
    }

    static void PrintEnvironment(IMSys2Environment environment, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.DarkYellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("Name: {0}", environment.Name);
        Console.ResetColor();
        Console.WriteLine();

        Console.Write(padding);
        Console.WriteLine("Installation path: {0}", environment.InstallationPath);

        Console.Write(padding);
        Console.WriteLine("Product path: {0}", environment.SetupInstance.ResolvePath(environment.ProductPath));
    }
}
