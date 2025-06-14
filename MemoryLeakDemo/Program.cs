// See https://aka.ms/new-console-template for more information
using MemoryLeakDemo;

Console.WriteLine("Hello, World!");
static void Main(string[] args)
{
    CreateLeakyObjects();

    Console.WriteLine("Forcing GC...");
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    Console.WriteLine("Done. Press Enter to exit.");
    Console.ReadLine();
}

static void CreateLeakyObjects()
{
    for (int i = 0; i < 100; i++)
    {
        var leak = new LeakyClass();
    }
}