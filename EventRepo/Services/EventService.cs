using EventRepo.Interfaces;
using EventRepo.Models;
using EventRepo.Models.DTO;
using System.Collections.Concurrent;

namespace EventRepo.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public PaginatedResult<Event> GetAllEvents(string? title = null, DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 10)
        {
            return _eventRepository.GetAllEvents(title, from, to, page, pageSize);
        }

        public Event? GetEventById(Guid id)
        {
            return _eventRepository.GetEventById(id);
        }

        public Guid? CreateEvent(EventDTO eventDTO)
        {
            return _eventRepository.CreateEvent(eventDTO);
        }

        public bool UpdateEvent(Guid id, EventDTO eventDTO)
        {
            return _eventRepository.UpdateEvent(id, eventDTO);
        }

        public bool DeleteEvent(Guid id)
        {
            return _eventRepository.DeleteEvent(id);
        }
    }
}
