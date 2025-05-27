using NUnit.Framework;
using IntelSyncLegacy.Models;
using IntelSyncLegacy.Services;

namespace IntelSyncLegacy.Tests;

public class BatchSyncProcessorTests
{
    [Test]
    public void ProcessAll_ShouldSetStatusToFailed_IfTokenIsMissing()
    {
        var jobs = new List<SyncJob>
        {
            new SyncJob
            {
                Id = 1,
                User = new CrmUser { Email = "invalid@example.com", Platform = "Salesforce", Token = "" },
                ObjectType = "Contact",
                Payload = "{}",
                SyncTime = DateTime.UtcNow
            }
        };

        var validator = new SimpleTokenValidator();
        var processor = new BatchSyncProcessor(jobs, validator);
        processor.ProcessAll();

        Assert.AreEqual("Failed", jobs[0].Status);
        Assert.AreEqual("Missing or invalid CRM token.", jobs[0].ErrorMessage);
    }
}
