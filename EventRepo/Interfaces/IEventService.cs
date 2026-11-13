using EventRepo.Models;
using EventRepo.Models.DTO;

namespace EventRepo.Interfaces
{
    public interface IEventService
    {
        IReadOnlyList<Event> GetAllEvents();
        Event? GetEventById(Guid id);
        Guid? CreateEvent(EventDTO eventDTO);
        bool UpdateEvent(Guid id, EventDTO eventDTO);
        bool DeleteEvent(Guid id);
    }
}
