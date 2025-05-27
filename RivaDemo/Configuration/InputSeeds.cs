using RivaDemo.Models;

namespace RivaDemo.Configuration
{
    //Static class for seeding data to Program
    public static class InputSeeds
    {
        public static List<SyncJob> GetSeedJobs()
        {
            return new List<SyncJob>
            {
                new SyncJob
                {
                    Id = 1,
                    User = new CrmUser
                    {
                        Email = "alice@example.com",
                        Platform = "Salesforce",
                        Token = "token123"
                    },
                    ObjectType = "Contact",
                    Payload = "{ \"email\": \"alice@example.com\", \"name\": \"Alice\" }",
                    SyncTime = DateTime.UtcNow,
                    Status = "Pending"
                },
                new SyncJob
                {
                    Id = 2,
                    User = new CrmUser
                    {
                        Email = "bob@example.com",
                        Platform = "Outlook",
                        Token = "" // Invalid Token
                    },
                    ObjectType = "Meeting",
                    Payload = "{ \"subject\": \"Weekly Sync\", \"organizer\": \"bob@example.com\" }",
                    SyncTime = DateTime.UtcNow,
                    Status = "Pending"
                }
            };
        }
    }
}
