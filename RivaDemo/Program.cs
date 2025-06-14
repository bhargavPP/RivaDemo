using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RivaDemo.Configuration;
using RivaDemo.Models;
using RivaDemo.Services;
using RivaDemo.Services.Interfaces;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Starting Synching data");

// ----------------------------------------------
// Application Entry Point
// - Configures Dependency Injection (DI) container
// - Seeds job data from InputSeeds
// - Registers service implementations
// - Resolves IBatchSyncProcessor and executes ProcessAll()
// ----------------------------------------------
try
{

    var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register seed data
        services.AddSingleton<List<SyncJob>>(provider => InputSeeds.GetSeedJobs());

        // Register services for DI
        services.AddSingleton<ISyncValidator, SimpleTokenValidator>();
        services.AddScoped<IBatchSyncProcessor, BatchSyncProcessor>();
    })
    .Build();

    var processor = host.Services.GetRequiredService<IBatchSyncProcessor>();


    processor.ProcessAll();


    
}
catch (Exception ex)
{
    Console.WriteLine($"[Fatal Error] An error occurred during startup or processing: {ex.Message}");
}