using IntelSyncLegacy.Models;

namespace IntelSyncLegacy.Services;

public class BatchSyncProcessor
{
    private readonly List<SyncJob> _jobs;
    private readonly ISyncValidator _validator;

    public BatchSyncProcessor(List<SyncJob> jobs, ISyncValidator validator)
    {
        _jobs = jobs;
        _validator = validator;
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
