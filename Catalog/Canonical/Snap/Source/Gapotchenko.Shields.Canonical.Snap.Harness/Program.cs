using Gapotchenko.FX.Linq;
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
            Run1();
            Run2();
            Run3();
        }
        catch (Exception e)
        {
            var writer = Console.Error;
            writer.Write("Error: ");
            writer.WriteLine(e);
        }
    }

    static void Run1()
    {
        Console.WriteLine("*** Canonical Snap Setup Instances ***");
        Console.WriteLine();

        var query = SnapDeployment.EnumerateSetupInstances();

        foreach (var (i, instance) in Enumerable.Range(1, int.MaxValue).Zip(query))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(ISnapSetupInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

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

        var query =
            SnapDeployment.EnumerateSetupInstances()
            .Select(SnapManagement.CreateManager)
            .SelectMany(manager =>
                manager.EnumeratePackages(SnapPackageListingOptions.Current)
                .Select(package => (manager, package)));

        Console.WriteLine("Installed snap packages of current revisions:");
        foreach (var (i, (manager, package)) in Enumerable.Range(1, int.MaxValue).Zip(query))
        {
            Console.WriteLine("{0}. {1}. Path: '{2}'.", i, package, manager.GetPackagePath(package));
        }
    }

    static void Run3()
    {
        Console.WriteLine("*** Canonical Snap Resolution ***");
        Console.WriteLine();

        string[] paths =
        [
            "/snap/bin/dotnet",
            "/snap/bin/firefox",
            "/snap/bin/example"
        ];

        Console.WriteLine("Resolving real file paths:");
        foreach (var (i, path) in Enumerable.Range(1, int.MaxValue).Zip(paths))
            Console.WriteLine("{0}. {1} -> {2}", i, path, SnapResolution.GetRealFilePath(path));
    }
}
