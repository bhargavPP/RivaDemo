using Moq;
using RivaDemo.Services.Interfaces;

namespace TestProject.Infrastructor
{
    public class Fixture
    {
        public readonly DataFactory DataFactory;
        public readonly Mock<IBatchSyncProcessor> BatchSynchProcessor;
        public Fixture()
        {

            BatchSynchProcessor = new Mock<IBatchSyncProcessor>();
            DataFactory = DataFactory.GetInstance();
        }
    }
}
