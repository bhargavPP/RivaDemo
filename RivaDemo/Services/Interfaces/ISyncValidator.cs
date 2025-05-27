using RivaDemo.Models;

namespace RivaDemo.Services.Interfaces;

public interface ISyncValidator
{
    bool IsValid(SyncJob job, out string errorMessage);
}
