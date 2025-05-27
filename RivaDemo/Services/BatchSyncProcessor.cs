using RivaDemo.Models;
using RivaDemo.Services.Interfaces;

namespace RivaDemo.Services;

public class BatchSyncProcessor : IBatchSyncProcessor
{
    private readonly List<SyncJob> _jobs;
    private readonly ISyncValidator _validator;

    public BatchSyncProcessor(List<SyncJob> jobs, ISyncValidator validator)
    {
        _jobs = jobs??throw new ArgumentNullException(nameof(jobs));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public void ProcessAll()
    {
        foreach (var job in _jobs)
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
    }
}
