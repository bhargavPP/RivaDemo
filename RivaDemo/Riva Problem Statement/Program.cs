using IntelSyncLegacy.Models;
using IntelSyncLegacy.Services;

var jobs = new List<SyncJob>
{
    new SyncJob
    {
        Id = 1,
        User = new CrmUser { Email = "alice@example.com", Platform = "Salesforce", Token = "token123" },
        ObjectType = "Contact",
        Payload = "{ \"email\": \"alice@example.com\", \"name\": \"Alice\" }",
        SyncTime = DateTime.UtcNow,
        Status = "Pending"
    },
    new SyncJob
    {
        Id = 2,
        User = new CrmUser { Email = "bob@example.com", Platform = "Outlook", Token = "" },
        ObjectType = "Meeting",
        Payload = "{ \"subject\": \"Weekly Sync\", \"organizer\": \"bob@example.com\" }",
        SyncTime = DateTime.UtcNow,
        Status = "Pending"
    }
};

var validator = new SimpleTokenValidator();
var processor = new BatchSyncProcessor(jobs, validator);
processor.ProcessAll();
