using RivaDemo.Models;
using RivaDemo.Services.Interfaces;
using System.Diagnostics;

namespace RivaDemo.Services;
// ----------------------------------------------
// BatchSyncProcessor
// - Implements IBatchSyncProcessor
// - Processes a batch of SyncJob items
// - Validates each job using ISyncValidator
// - Updates job status and logs result
// ----------------------------------------------

public class BatchSyncProcessor : IBatchSyncProcessor
{
    private readonly List<SyncJob> _jobs;
    private readonly ISyncValidator _validator;

    public BatchSyncProcessor(List<SyncJob> jobs, ISyncValidator validator)
    {
        _jobs = jobs ?? throw new ArgumentNullException(nameof(jobs));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    ///<inheritdoc cref="IBatchSyncProcessor.ProcessAll"/>
    public void ProcessAll()
    {

        foreach (var job in _jobs)
        {
            try
            {
                Console.WriteLine($"[Sync] {job.User.Email} - {job.ObjectType} via {job.User.Platform}");

                if (!_validator.IsValid(job, out var error))
                {
                    job.Status = "Failed";
                    job.ErrorMessage = error;
                    Console.WriteLine($"[Error] {error}");
                    continue;
                }

                job.Status = "Success";
                Console.WriteLine($"[OK] Synced {job.ObjectType} for {job.User.Email}");
            }
            catch (Exception ex)
            {
                job.Status = "Failed";
                job.ErrorMessage = $"Unexpected error: {ex.Message}";
                Console.WriteLine($"[Exception] Failed to sync {job.User.Email}: {ex.Message}");
            }
        }

    }
}
