using IntelSyncLegacy.Models;

namespace IntelSyncLegacy.Services;

public interface ISyncValidator
{
    bool IsValid(SyncJob job, out string errorMessage);
}
