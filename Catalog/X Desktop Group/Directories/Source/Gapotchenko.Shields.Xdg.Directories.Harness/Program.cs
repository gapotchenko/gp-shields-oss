using Gapotchenko.Shields.Xdg.Directories.Base;
using Gapotchenko.Shields.Xdg.Directories.User;

namespace Gapotchenko.Shields.Xdg.Directories.Harness;

class Program
{
    static void Main()
    {
        try
        {
            Run1();
            Console.WriteLine();

            Run2();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void Run1()
    {
        Console.WriteLine("*** XDG Base Directories ***");
        Console.WriteLine();

        foreach (var directory in XdgBaseDirectory.Enumerate())
            Console.WriteLine("{0}: {1}", directory, directory.Value);
    }

    static void Run2()
    {
        Console.WriteLine("*** XDG User Directories ***");
        Console.WriteLine();

        foreach (var directory in XdgUserDirectory.Enumerate())
            Console.WriteLine("{0}: {1}", directory, directory.ValueOrDefault);
    }
}
