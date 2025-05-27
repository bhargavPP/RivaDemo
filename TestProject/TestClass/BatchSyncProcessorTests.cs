using RivaDemo.Models;
using RivaDemo.Services;
using System.Text.Json;

namespace TestProject.TestClass;

/// <summary>
/// BatchSyncProcessorTests
/// -----------------------
/// Unit tests for BatchSyncProcessor functionality.
/// - Loads test cases from external JSON file.
/// - Validates sync behavior based on token presence.
/// - Asserts expected status and error handling for each job.
/// 
/// Structure:
/// TestCases/BatchSyncProcessorTestsCases.json -> holds mock job data
/// This file simulates different job scenarios for input.
/// </summary>

public class BatchSyncProcessorTests
{
    private static readonly string JsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestCases", "BatchSyncProcessorTestsCases.json");
   
    /// <summary>
    /// Reads and deserializes test jobs from JSON file.
    /// Ensures consistent, repeatable test inputs from a separate file.
    /// </summary>
    private List<SyncJob> LoadTestJobs()
    {
        var json = File.ReadAllText(JsonFilePath);
        var jobs = JsonSerializer.Deserialize<List<SyncJob>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return jobs!;
    }

    /// <summary>
    /// Test Case:
    /// Verifies that a sync job fails if the user token is missing or invalid.
    /// - Loads job data from JSON
    /// - Executes BatchSyncProcessor
    /// - Asserts job status and error message
    /// </summary>

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

    /// <summary>
    /// Test Case:
    /// Verifies that a sync job success if the user token is valid.
    /// - Loads job data from JSON
    /// - Executes BatchSyncProcessor
    /// - Asserts job status and error message
    /// </summary>
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
