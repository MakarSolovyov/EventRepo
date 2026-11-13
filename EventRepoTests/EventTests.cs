using EventRepo.Interfaces;
using EventRepo.Models.DTO;
using EventRepo.Services;
using Moq;
using System.Timers;

namespace EventRepoTests
{
    public class EventFixture
    {
        public Mock<IEventRepository> MockRepo { get; set; }
        public EventService EventService { get; set; }

        public EventFixture()
        {
            MockRepo = new Mock<IEventRepository>();
            EventService = new EventService(MockRepo.Object);
        }
    }

    public class EventTests : IClassFixture<EventFixture>
    {
        private readonly Mock<IEventRepository> _mockRepo;
        private readonly EventService _eventService;

        public EventTests(EventFixture eventFixture)
        {
            _mockRepo = eventFixture.MockRepo;
            _eventService = eventFixture.EventService;
        }

        [Fact]
        public void CreateEvent_RepoReturnsEvent_ReturnsNewEvent()
        {
            var newEvent = new EventDTO()
            {
                Title = "Новое событие",
                Description = "Описание события",
                StartAt = new DateTime(2026, 1, 1, 10, 0, 0),
                EndAt = new DateTime(2026, 1, 1, 12, 0, 0)
            };

            var expectedId = Guid.NewGuid();

            _mockRepo.Setup(method => method.CreateEvent(It.IsAny<EventDTO>())).Returns(expectedId);

            var result = _eventService.CreateEvent(newEvent);

            _mockRepo.Verify(repository => repository.CreateEvent(It.IsAny<EventDTO>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public void CreateEvent_RepoReturnsNull_ReturnsNull()
        {
            var newEvent = new EventDTO()
            {
                Title = "Новое событие",
                Description = "Описание события",
                StartAt = new DateTime(2026, 1, 1, 10, 0, 0),
                EndAt = new DateTime(2026, 1, 1, 12, 0, 0)
            };

            var newEventId = Guid.NewGuid();

            _mockRepo.Setup(method => method.CreateEvent(It.IsAny<EventDTO>())).Returns((Guid?)null);

            var result = _eventService.CreateEvent(newEvent);

            _mockRepo.Verify(repository => repository.CreateEvent(It.IsAny<EventDTO>()), Times.Once);
            Assert.Null(result);
        }
    }
}
