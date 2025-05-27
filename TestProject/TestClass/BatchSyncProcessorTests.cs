using RivaDemo.Models;
using RivaDemo.Services;
using System.Text.Json;

namespace TestProject.TestClass;

public class BatchSyncProcessorTests
{
    private static readonly string JsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestCases", "BatchSyncProcessorTestsCases.json");

    private List<SyncJob> LoadTestJobs()
    {
        var json = File.ReadAllText(JsonFilePath);
        var jobs = JsonSerializer.Deserialize<List<SyncJob>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return jobs!;
    }

    [Test]
    public void ProcessAll_ShouldMarkJobAsFailed_WhenTokenIsMissing()
    {
        // Arrange
        var jobs = LoadTestJobs();
        var validator = new SimpleTokenValidator();
        var processor = new BatchSyncProcessor(jobs, validator);

        // Act
        processor.ProcessAll();

        // Assert
        var failedJob = jobs.FirstOrDefault(j => j.User.Email == "bob@example.com");
        Assert.NotNull(failedJob);
        Assert.AreEqual("Failed", failedJob.Status);
        Assert.AreEqual("Missing or invalid CRM token.", failedJob.ErrorMessage);
    }

    [Test]
    public void ProcessAll_ShouldMarkJobAsSuccess_WhenTokenIsPresent()
    {
        // Arrange
        var jobs = LoadTestJobs();
        var validator = new SimpleTokenValidator();
        var processor = new BatchSyncProcessor(jobs, validator);

        //Act
        processor.ProcessAll();

        // Assert
        var successJob = jobs.FirstOrDefault(j => j.User.Email == "alice@example.com");
        Assert.NotNull(successJob);
        Assert.AreEqual("Success", successJob.Status);
        Assert.IsTrue(string.IsNullOrEmpty(successJob.ErrorMessage));
    }

}
