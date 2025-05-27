using RivaDemo.Models;

namespace RivaDemo.Services.Interfaces;

// ----------------------------------------------
// ISyncValidator
// - Validates a jobs 
// - Sets error message on failure
// ----------------------------------------------
public interface ISyncValidator
{

    /// <summary>
    ///  validating a sync job before processing
    ///  Returns a boolean for success/failure
    //   Outputs error message if validation fails
    //   Enables interchangeable validation strategies
    /// </summary>
    /// <param name="job"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>

    bool IsValid(SyncJob job, out string errorMessage);
}
