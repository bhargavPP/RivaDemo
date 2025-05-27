namespace RivaDemo.Services.Interfaces;
// ----------------------------------------------
// IBatchSyncProcessor
// - Interface for batch processing of sync jobs
// - Encapsulates the logic to iterate and handle multiple jobs
// - Used to decouple processing logic from its implementation
// - Supports dependency injection and testability
// ----------------------------------------------
public interface IBatchSyncProcessor
{
    /// <summary>
    /// Processes a batch of SyncJob items
    /// Validates each job using ISyncValidator
    /// Updates job status and logs result
    /// </summary>
    /// <returns></returns>
    void ProcessAll();
}

