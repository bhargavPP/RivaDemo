using IntelSyncLegacy.Models;

namespace IntelSyncLegacy.Services;

public class SimpleTokenValidator : ISyncValidator
{
    public bool IsValid(SyncJob job, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(job.User?.Token))
        {
            errorMessage = "Missing or invalid CRM token.";
            return false;
        }

        return true;
    }
}
