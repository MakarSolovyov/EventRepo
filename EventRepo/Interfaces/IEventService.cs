using EventRepo.Models;
using EventRepo.Models.DTO;

namespace EventRepo.Interfaces
{
    public interface IEventService
    {
        PaginatedResult<Event> GetAllEvents(
            string? title = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 10);
        Event? GetEventById(Guid id);
        Event? CreateEvent(EventDTO eventDTO);
        bool UpdateEvent(Guid id, EventDTO eventDTO);
        bool DeleteEvent(Guid id);
    }
}
