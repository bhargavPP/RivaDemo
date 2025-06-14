using GCalanderSync.Services.Interface;
using Moq;
using NUnit.Framework.Legacy;

namespace TestProject.TestClass
{
    [TestFixture]
    public class GoogleCalendarServiceTests
    {
        [Test]
        public async Task GetUpcomingEventsAsync_ReturnEvents()
        {
            // Arrange
            var mockService = new Mock<IGoogleCalendarService>();

            var fakeEvents = new List<string>
    {
        "2025-06-14T10:00:00 - Meeting with Team",
        "2025-06-15T15:00:00 - Client Webinar"
    };

            mockService
                .Setup(s => s.GetUpcomingEventsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(fakeEvents);

            // Act
            var events = await mockService.Object.GetUpcomingEventsAsync("accessToken", "refreshToken", "clientId", "clientSecret");

            // Assert
            Assert.That(events, Is.Not.Null);
            CollectionAssert.AreEqual(fakeEvents, events);
        }
    }
}
