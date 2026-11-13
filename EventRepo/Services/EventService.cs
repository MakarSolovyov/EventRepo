using EventRepo.Interfaces;
using EventRepo.Models;
using EventRepo.Models.DTO;
using System.Collections.Concurrent;

namespace EventRepo.Services
{
    public class EventService : IEventService
    {
        private readonly ConcurrentDictionary<Guid, Event> _events = new();

        public PaginatedResult<Event> GetAllEvents(
            string? title = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 10) 
        {
            IEnumerable<Event> filteredEvents = _events.Values;

            if (!string.IsNullOrWhiteSpace(title))
                filteredEvents = filteredEvents.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (from.HasValue)
                filteredEvents = filteredEvents.Where(e => e.StartAt >= from.Value);

            if (to.HasValue)
                filteredEvents = filteredEvents.Where(e => e.EndAt <= to.Value);

            var totalCount = filteredEvents.Count();

            filteredEvents = filteredEvents.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new PaginatedResult<Event>()
            {
                TotalCount = totalCount,
                Items = filteredEvents,
                Page = page,
                PageSize = filteredEvents.Count()
            };

            return result;
        }

        public Event? GetEventById(Guid id) 
        {
            return _events.TryGetValue(id, out var ev)
                ? ev
                : null;
        }

        public Guid? CreateEvent(EventDTO eventDTO) 
        {
            var newEvent = ToEvent(eventDTO);

            if (_events.TryAdd(newEvent.Id, newEvent))
                return newEvent.Id;

            return null;
        }

        public bool UpdateEvent(Guid id, EventDTO eventDTO) 
        {
            var newEvent = ToEvent(eventDTO, id);

            while (_events.TryGetValue(newEvent.Id, out var current))
            {
                if (_events.TryUpdate(newEvent.Id, newEvent, current))
                    return true;
            }

            return false;
        }

        public bool DeleteEvent(Guid id) 
        {
            return _events.TryRemove(id, out _);
        }

        private Event ToEvent(EventDTO eventDTO, Guid? id = null) 
        {
            return  new Event(
                id ?? Guid.NewGuid(),
                eventDTO.Title,
                eventDTO.Description,
                eventDTO.StartAt.Value,
                eventDTO.EndAt.Value);
        }
    }
}
