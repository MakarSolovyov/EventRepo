using EventRepo.Interfaces;
using EventRepo.Models;
using EventRepo.Models.DTO;
using EventRepo.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Moq;
using System.Net;
using System.Timers;

namespace EventRepoTests
{
    public class EventTests
    {
        private readonly EventService _eventService;
        private readonly List<Event> _events;

        public EventTests()
        {
            _events =
            [
                new Event(
                    Guid.Parse("7f3a9c2e-4b1d-4e8f-9a6c-2d5e8b1f3a7c"),
                    "Новое событие 1",
                    "Описание события 1",
                    new DateTime(2026, 1, 1, 10, 0, 0),
                    new DateTime(2026, 1, 1, 11, 0, 0)),
                new Event(
                    Guid.Parse("c4e2a8b6-1f3d-4a9e-8b7c-5d2e6f1a3c9b"),
                    "Новое событие 2",
                    "Описание события 2",
                    new DateTime(2026, 1, 1, 11, 0, 0),
                    new DateTime(2026, 1, 1, 12, 0, 0)),
                new Event(
                    Guid.Parse("2a8f5c1e-9d4b-4f7a-b3e6-8c1d5a2f7e4b"),
                    "Новое событие 3",
                    "Описание события 3",
                    new DateTime(2026, 1, 1, 12, 0, 0),
                    new DateTime(2026, 1, 1, 13, 0, 0))
            ];

            _eventService = new EventService(_events);
        }

        [Fact]
        public void CreateEvent_NewId_ShouldReturnNewEvent()
        {
            var expectedEvent = new EventDTO()
            {
                Title = "Новое событие",
                Description = "Описание события",
                StartAt = new DateTime(2026, 1, 1, 10, 0, 0),
                EndAt = new DateTime(2026, 1, 1, 13, 0, 0)
            };
            var expectedTitle = "Новое событие";

            var result = _eventService.CreateEvent(expectedEvent);

            Assert.NotNull(result);

            var persisted = _eventService.GetEventById(result!.Id);
            Assert.NotNull(persisted);
            Assert.Equal(expectedTitle, persisted!.Title);
        }

        [Fact]
        public void GetAllEvents_ShouldReturnEventCollection() 
        {
            var expectedTotalCount = 3;
            var expectedItemsCountOnPage = 3;
            var expectedEvents = new List<Event>()
            {
                new Event(
                    Guid.Parse("7f3a9c2e-4b1d-4e8f-9a6c-2d5e8b1f3a7c"),
                    "Новое событие 1",
                    "Описание события 1",
                    new DateTime(2026, 1, 1, 10, 0, 0),
                    new DateTime(2026, 1, 1, 11, 0, 0)),
                new Event(
                    Guid.Parse("c4e2a8b6-1f3d-4a9e-8b7c-5d2e6f1a3c9b"),
                    "Новое событие 2",
                    "Описание события 2",
                    new DateTime(2026, 1, 1, 11, 0, 0),
                    new DateTime(2026, 1, 1, 12, 0, 0)),
                new Event(
                    Guid.Parse("2a8f5c1e-9d4b-4f7a-b3e6-8c1d5a2f7e4b"),
                    "Новое событие 3",
                    "Описание события 3",
                    new DateTime(2026, 1, 1, 12, 0, 0),
                    new DateTime(2026, 1, 1, 13, 0, 0))
            };

            var result = _eventService.GetAllEvents();

            Assert.Equal(result.Items.Select(e => e.Id).OrderBy(id => id),
                expectedEvents.Select(e => e.Id).OrderBy(id => id));
            Assert.Equal(expectedTotalCount, result.TotalCount);
            Assert.Equal(expectedItemsCountOnPage, result.Items.Count());
        }

        [Fact]
        public void GetEventById_ExistingId_ShouldReturnEvent()
        {
            var expectedEvent = new Event(
                    Guid.Parse("7f3a9c2e-4b1d-4e8f-9a6c-2d5e8b1f3a7c"),
                    "Новое событие 1",
                    "Описание события 1",
                    new DateTime(2026, 1, 1, 10, 0, 0),
                    new DateTime(2026, 1, 1, 11, 0, 0));

            var result = _eventService.GetEventById(expectedEvent.Id);

            Assert.NotNull(result);
            Assert.Equivalent(result, expectedEvent);
        }

        [Fact]
        public void GetEventById_NewId_ShouldReturnNull()
        {
            var newEventId = Guid.NewGuid();

            var result = _eventService.GetEventById(newEventId);

            Assert.Null(result);
        }

        [Fact]
        public void UpdateEvent_ExistingId_ShouldReturnTrueAndUpdateFields()
        {
            var expectedEventToUpdate = new Event(
                    Guid.Parse("7f3a9c2e-4b1d-4e8f-9a6c-2d5e8b1f3a7c"),
                    "Новое событие 1",
                    "Описание события 1",
                    new DateTime(2026, 1, 1, 10, 0, 0),
                    new DateTime(2026, 1, 1, 11, 0, 0));
            var expectedEventUpdateInfo = new EventDTO()
            {
                Title = "Новое событие",
                Description = "Описание события",
                StartAt = new DateTime(2026, 1, 1, 10, 0, 0),
                EndAt = new DateTime(2026, 1, 1, 13, 0, 0)
            };

            var result = _eventService.UpdateEvent(expectedEventToUpdate.Id, expectedEventUpdateInfo);

            Assert.True(result);
            var updated = _eventService.GetEventById(expectedEventToUpdate.Id);
            Assert.NotNull(updated);
            Assert.Equal(expectedEventUpdateInfo.Title, updated!.Title);
        }

        [Fact]
        public void UpdateEvent_UnknownId_ShouldReturnFalse()
        {
            var newEventId = Guid.NewGuid();
            var expectedEventUpdate = new EventDTO()
            {
                Title = "Новое событие",
                Description = "Описание события",
                StartAt = new DateTime(2026, 1, 1, 10, 0, 0),
                EndAt = new DateTime(2026, 1, 1, 13, 0, 0)
            };

            var result = _eventService.UpdateEvent(newEventId, expectedEventUpdate);

            Assert.False(result);
        }

        [Fact]
        public void DeleteEvent_ExistingId_ShouldReturnTrueAndRemove()
        {
            var expectedEventDeleteId = Guid.Parse("c4e2a8b6-1f3d-4a9e-8b7c-5d2e6f1a3c9b");

            var result = _eventService.DeleteEvent(expectedEventDeleteId);

            Assert.True(result);
            Assert.Null(_eventService.GetEventById(expectedEventDeleteId));
        }

        [Fact]
        public void DeleteEvent_UnknownIdShouldReturnFalse()
        {
            var newEventId = Guid.NewGuid();

            var result = _eventService.DeleteEvent(newEventId);

            Assert.False(result);
        }

        [Fact]
        public void GetAllEvents_FilterByTitle_ShouldReturnMatches()
        {
            var expectedEventCount = 1;
            var expectedTitlePart = "событие 1";

            var result = _eventService.GetAllEvents(title: expectedTitlePart);

            Assert.Equal(expectedEventCount, result.TotalCount);
            Assert.Contains(result.Items, e => e.Title.Contains(expectedTitlePart));
        }

        [Fact]
        public void GetAllEvents_FilterDate_ShouldReturnMatches()
        {
            var expectedEventCount = 3;
            var expectedFromDate = new DateTime(2026, 1, 1, 10, 0, 0);
            
            var result = _eventService.GetAllEvents(from: expectedFromDate);

            Assert.Equal(expectedEventCount, result.TotalCount);
            Assert.Contains(result.Items, e => e.StartAt >= expectedFromDate);
        }

        [Fact]
        public void GetAllEvents_Pagination_ShouldReturnCorrectPage()
        {
            for (int i = 4; i < 15; i++)
            {
                _eventService.CreateEvent(new EventDTO()
                {
                    Title = $"Новое событие {i}",
                    Description = $"Описание события {i}",
                    StartAt = new DateTime(2026, 1, 1, 10, 0, 0),
                    EndAt = new DateTime(2026, 1, 1, 13, 0, 0)
                });
            }
            var expectedTotalCount = 14;
            var expectedPage1EventCount = 5;
            var expectedPage3EventCount = 4;

            var page1 = _eventService.GetAllEvents(page: 1, pageSize: 5);
            var page3 = _eventService.GetAllEvents(page: 3, pageSize: 5);

            Assert.Equal(expectedTotalCount, page1.TotalCount);
            Assert.Equal(expectedPage1EventCount, page1.Items.Count());
            Assert.Equal(expectedPage3EventCount, page3.Items.Count());
        }

        [Fact]
        public void GetAllEvents_CombineFilter_ShouldReturnMatches()
        {
            var expectedEventCount = 1;
            var expectedTitlePart = "событие 1";
            var expectedFromDate = new DateTime(2026, 1, 1, 10, 0, 0);
            var expectedToDate = new DateTime(2026, 1, 1, 12, 0, 0);

            var result = _eventService.GetAllEvents(title: expectedTitlePart, from: expectedFromDate, to: expectedToDate); 

            Assert.Equal(expectedEventCount, result.TotalCount);
            Assert.Contains(result.Items, e => e.Title.Contains(expectedTitlePart));
            Assert.Contains(result.Items, e => e.StartAt >= expectedFromDate);
            Assert.Contains(result.Items, e => e.EndAt <= expectedToDate);
        }


    }
}
