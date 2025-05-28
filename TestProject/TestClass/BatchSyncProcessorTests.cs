using RivaDemo.Models;
using RivaDemo.Services;
using System.Text.Json;
using TestProject.Infrastructor;

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
    private readonly Fixture _fixture;
    private readonly DataFactory _dataFactory;
    public BatchSyncProcessorTests()
    {
        _fixture = new Fixture();
        _dataFactory = _fixture.DataFactory;
    }

    //[SetUp]
    //public void SetUp()
    //{
    //    _dataFactory.Reset(); // Ensure clean state for each test
    //}


    /// <summary>
    /// Test Case:
    /// Verifies that the JSON file loads correctly and contains expected jobs.
    /// </summary>
    [Test]
    public void DataFactory_ShouldLoadJobsFromJson()
    {
        // Act
        var jobs = _dataFactory.GetAllSyncJobs();

        // Assert
        Assert.IsNotEmpty(jobs, "No jobs loaded from JSON file.");
        Assert.IsTrue(jobs.Any(j => j.User?.Email == "alice@example.com"), "Expected job with email 'alice@example.com' not found in JSON data.");
        Assert.IsTrue(jobs.Any(j => j.User?.Email == "bob@example.com"), "Expected job with email 'bob@example.com' not found in JSON data.");
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
        var jobs = _dataFactory.GetAllSyncJobs(); // Load from JSON
        var validator = new SimpleTokenValidator();
        var processor = new BatchSyncProcessor(jobs, validator);

        // Act
        processor.ProcessAll();

        // Assert
        var failedJob = jobs.FirstOrDefault(j => j.User.Email == "bob@example.com");
        Assert.NotNull(failedJob, "Expected job with email 'bob@example.com' not found.");
        Assert.AreEqual("Failed", failedJob.Status, "Job status should be 'Failed'.");
        Assert.AreEqual("Missing or invalid CRM token.", failedJob.ErrorMessage, "Expected error message not set.");
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
        var jobs = _dataFactory.GetAllSyncJobs(); // Load from JSON
        var validator = new SimpleTokenValidator();
        var processor = new BatchSyncProcessor(jobs, validator);

        //Act
        processor.ProcessAll();

        // Assert
        var successJob = jobs.FirstOrDefault(j => j.User.Email == "alice@example.com");
        Assert.NotNull(successJob, "Expected job with email alice@example.com not found.");
        Assert.AreEqual("Success", successJob.Status, "Job status should be 'Success'.");
        Assert.IsTrue(string.IsNullOrEmpty(successJob.ErrorMessage), "Error message should be empty.");

    }

}
