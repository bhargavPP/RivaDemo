using RivaDemo.Models;
using System.Text.Json;

namespace TestProject.Infrastructor
{
    /// <summary>
    /// DataFactory
    /// -----------
    /// Provides mock job data for testing purposes.
    /// 
    /// Note:
    /// This class simulates data loading from a JSON file.
    /// In production or advanced test scenarios, this can be replaced with a mock of DbContext or an in-memory database.
    /// </summary>
    public class DataFactory
    {

        private static readonly string JsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestCases", "BatchSyncProcessorTestsCases.json");
        private static readonly object _lock = new object();

        private static DataFactory instance;
        private readonly List<SyncJob> syncJobs;

        // Private constructor for singleton
        private DataFactory()
        {

            syncJobs = new List<SyncJob>();
            LoadTestJobs();
        }

        /// <summary>
        /// Thread-safe singleton instance retrieval.
        /// </summary>
        public static DataFactory GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new DataFactory();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// Reads and deserializes test jobs from JSON file.
        /// Ensures consistent, repeatable test inputs from a separate file.
        /// </summary>
        private void LoadTestJobs()
        {
            try
            {
                if (!File.Exists(JsonFilePath))
                {
                    throw new FileNotFoundException($"JSON file not found at: {JsonFilePath}");
                }

                var json = File.ReadAllText(JsonFilePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("JSON file is empty.");
                }

                var jobs = JsonSerializer.Deserialize<List<SyncJob>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (jobs == null || !jobs.Any())
                {
                    throw new InvalidOperationException("No jobs loaded from JSON file.");
                }

                syncJobs.Clear(); // Clear existing jobs before loading
                syncJobs.AddRange(jobs);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load test jobs from {JsonFilePath}: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Gets all SyncJobs from the JSON file or generated data.
        /// </summary>
        public List<SyncJob> GetAllSyncJobs()
        {
            return syncJobs.ToList();
        }

        /// <summary>
        /// Generates a list of SyncJobs for testing BatchSyncProcessor.
        /// </summary>
        /// <param name="count">Number of jobs to generate.</param>
        /// <param name="status">Optional status for all jobs.</param>
        /// <param name="isValid">Whether to generate valid or invalid jobs.</param>
        public List<SyncJob> GenerateSyncJobs(int count, string status = null, bool isValid = true)
        {
            var jobs = new List<SyncJob>();
            var random = new Random();
            var platforms = new[] { "Salesforce", "Outlook" };

            for (int i = 0; i < count; i++)
            {
                var platform = platforms[random.Next(platforms.Length)];
                var email = $"test{random.Next(1000)}@example.com";
                var job = new SyncJob
                {
                    Id = syncJobs.Count + i + 1,
                    User = isValid ? new CrmUser
                    {
                        Email = email,
                        Platform = platform,
                        Token = platform == "Salesforce" ? $"token{random.Next(1000)}" : ""
                    } : null,
                    ObjectType = isValid ? (random.Next(2) == 0 ? "Contact" : "Meeting") : null,
                    Payload = isValid
                        ? platform == "Salesforce"
                            ? $"{{ \"email\": \"{email}\", \"name\": \"Test{random.Next(1000)}\" }}"
                            : $"{{ \"subject\": \"Weekly Sync {random.Next(1000)}\", \"organizer\": \"{email}\" }}"
                        : null,
                    SyncTime = DateTime.Now,
                    Status = status ?? (isValid ? "Pending" : "Invalid")
                };
                jobs.Add(job);
            }

            syncJobs.AddRange(jobs);
            return jobs;
        }

        /// <summary>
        /// Generates a single SyncJob with specific properties for edge case testing.
        /// </summary>
        /// <param name="email">Optional email for the User.</param>
        /// <param name="status">Job status, defaults to "Pending".</param>
        /// <param name="isValid">Whether to generate a valid job.</param>
        /// <param name="objectType">Object type, defaults to "Contact".</param>
        public SyncJob GenerateSingleSyncJob(string email = null, string status = "Pending", bool isValid = true, string objectType = "Contact")
        {
            var platform = objectType == "Contact" ? "Salesforce" : "Outlook";
            var userEmail = email ?? $"test{Guid.NewGuid().ToString("N").Substring(0, 8)}@example.com";
            var job = new SyncJob
            {
                Id = syncJobs.Count + 1,
                User = isValid ? new CrmUser
                {
                    Email = userEmail,
                    Platform = platform,
                    Token = platform == "Salesforce" ? "defaultToken" : ""
                } : null,
                ObjectType = isValid ? objectType : null,
                Payload = isValid
                    ? objectType == "Contact"
                        ? $"{{ \"email\": \"{userEmail}\", \"name\": \"Test\" }}"
                        : $"{{ \"subject\": \"Test Meeting\", \"organizer\": \"{userEmail}\" }}"
                    : null,
                SyncTime = DateTime.Now,
                Status = status
            };
            syncJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Clears all in-memory SyncJob data for test isolation.
        /// </summary>
        public void Reset()
        {
            syncJobs.Clear();
        }

        /// <summary>
        /// Gets the count of SyncJobs.
        /// </summary>
        public int GetSyncJobCount()
        {
            return syncJobs.Count;
        }
    }
}
