using Moq;
using RivaDemo.Services.Interfaces;

namespace TestProject.Infrastructor
{
    /// <summary>
    /// Fixture
    /// -------
    /// Provides shared test setup dependencies for unit tests.
    /// 
    /// Responsibilities:
    /// - Initializes a mock instance of IBatchSyncProcessor for injection.
    /// - Provides access to mock data via DataFactory.
    /// </summary>
    public class Fixture
    {
        /// <summary>
        /// Provides access to mock job data.
        /// Consider replacing with DbContext-based implementation for advanced testing.
        /// </summary>
        public readonly DataFactory DataFactory;

        // <summary>
        /// Mock implementation of IBatchSyncProcessor used for injecting into test scenarios.
        /// </summary>
        public readonly Mock<IBatchSyncProcessor> BatchSynchProcessor;

        /// <summary>
        /// Initializes the Fixture with mock services and test data providers.
        /// </summary>
        public Fixture()
        {

            BatchSynchProcessor = new Mock<IBatchSyncProcessor>();
            DataFactory = DataFactory.GetInstance();
        }
    }
}
