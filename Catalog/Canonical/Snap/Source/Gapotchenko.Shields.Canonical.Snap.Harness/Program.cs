using Gapotchenko.Shields.Canonical.Snap.Deployment;
using Gapotchenko.Shields.Canonical.Snap.Management;
using Gapotchenko.Shields.Canonical.Snap.Resolution;

namespace Gapotchenko.Shields.Canonical.Snap.Harness;

class Program
{
    static void Main()
    {
        try
        {
            //Run1();
            //Run2();
            Run3();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void Run1()
    {
        Console.WriteLine("*** Canonical Snap Setup Instances ***");
        Console.WriteLine();

        foreach (var (i, instance) in
            Enumerable.Range(1, int.MaxValue)
            .Zip(
                SnapDeployment.EnumerateSetupInstances(),
                ValueTuple.Create))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(ISnapSetupInstance instance, int level = 0)
    {
        var padding = new string(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "Canonical.Snap.Product");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write(padding);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);

        Console.Write(padding);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));

        Console.Write(padding);
        Console.WriteLine("Attributes: {0}", instance.Attributes);
    }

    static void Run2()
    {
        Console.WriteLine("*** Canonical Snap Packages ***");
        Console.WriteLine();

        foreach (var (i, (manager, packageName)) in
            Enumerable.Range(1, int.MaxValue)
            .Zip(
                SnapDeployment.EnumerateSetupInstances()
                .Select(SnapManagement.CreateManager)
                .SelectMany(m => m.EnumeratePackages(SnapPackageListingOptions.Current).Select(p => (m, p))),
                ValueTuple.Create))
        {
            Console.WriteLine("{0}. {1}. Path: '{2}'.", i, packageName, manager.GetPackagePath(packageName));
        }
    }

    static void Run3()
    {
        Console.WriteLine("*** Canonical Snap Resolution ***");
        Console.WriteLine();

        var paths = new[]
        {
            "/snap/bin/dotnet",
            "/snap/bin/firefox",
            "/snap/bin/example"
        };

        int i = 0;
        foreach (var path in paths)
        {
            Console.WriteLine("#{0}", ++i);
            Console.WriteLine(SnapResolver.GetRealFilePath(path));

            Console.WriteLine();
        }
    }
}
