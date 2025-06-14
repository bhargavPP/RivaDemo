using RivaDemo.Models;
using RivaDemo.Services.Interfaces;

namespace RivaDemo.Services;
// ----------------------------------------------
// SimpleTokenValidator
// - Implements ISyncValidator
// - Validates if a SyncJob contains a non-empty CRM token
// - Sets error message on failure
// ----------------------------------------------

public class SimpleTokenValidator : ISyncValidator
{
    /// <inheritdoc cref="ISyncValidator.IsValid(SyncJob, out string)"/>
    public bool IsValid(SyncJob job, out string errorMessage)
    {
        try
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(job.User?.Token))
            {
                errorMessage = "Missing or invalid CRM token.";
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"Validation failed due to unexpected error: {ex.Message}";
            return false;
        }
    }
}
