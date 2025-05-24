using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Microsoft.PowerShell.Deployment;
using Gapotchenko.Shields.Microsoft.PowerShell.Script;
using System.Text;

namespace Gapotchenko.Shields.Microsoft.PowerShell.Harness;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            await Run2();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void Run1()
    {
        Console.WriteLine("*** Microsoft PowerShell Setup Instances ***");
        Console.WriteLine();

        foreach (var (i, instance) in
            Enumerable.Range(1, int.MaxValue)
            .Zip(
                PowerShellDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>()),
                ValueTuple.Create))
        {
            Console.WriteLine("#{0}", i);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Product ID: {0}", instance.Product.Id);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Display name: {0}", instance.DisplayName);
            Console.WriteLine("Installation path: {0}", instance.InstallationPath);
            Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));

            Console.WriteLine();
        }
    }

    static async Task Run2()
    {
        string script =
            """
            Add-Type -AssemblyName PresentationCore,PresentationFramework
            $Result = [System.Windows.MessageBox]::Show("Hello from PowerShell script Привіт")
            """;

        var s =
            """
            Hello from PowerShell script
            Привіт
            """;

        script =
            $"""
            Add-Type -AssemblyName PresentationCore,PresentationFramework
            $Result = [System.Windows.MessageBox]::Show('[' + {PowerShellScript.QuoteString(s)} + ']')
            """;

        await PowerShellScript.ExecuteAsync(PowerShellDeployment.DefaultSetupInstance, script);
    }
}
