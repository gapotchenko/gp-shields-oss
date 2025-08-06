// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.BusyBox.Deployment;

namespace Gapotchenko.Shields.BusyBox.Harness;

class Program
{
    static void Main()
    {
        try
        {
            ListSetupInstances();
            ListPortableSetupInstances();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void ListSetupInstances()
    {
        Console.WriteLine("*** Installed Setup Instances ***");
        Console.WriteLine();

        foreach (var (instance, i) in
            BusyBoxDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())
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

        string[] paths =
        [
            @"C:\busybox", // as directory            
            @"C:\busybox\busybox.exe", @"C:\busybox\busybox64.exe", @"C:\busybox\busybox64u.exe", @"C:\busybox\busybox64a.exe" // as individual files
        ];

        foreach (var (path, i) in paths.Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0} at '{1}'", i, path);

            var instance = BusyBoxSetupInstance.TryOpen(path);
            if (instance is null)
                Console.WriteLine("BusyBox setup instance is not found.");
            else
                PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(IBusyBoxSetupInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "BusyBox.Product");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write(padding);
        Console.WriteLine("Display name: {0}", instance.DisplayName);

        Console.Write(padding);
        Console.WriteLine("Manufacturer version: {0}", instance.ManufacturerVersion);

        Console.Write(padding);
        Console.WriteLine("Architecture: {0}", instance.Architecture);

        Console.Write(padding);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);

        Console.Write(padding);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));

        Console.Write(padding);
        Console.WriteLine("Attributes: {0}", instance.Attributes);
    }
}
