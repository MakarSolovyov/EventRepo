using EventRepo.Models;

namespace EventRepo.Interfaces
{
    public interface IEventService
    {
        IReadOnlyList<Event> GetAllEvents();
        Event GetEventById(Guid id);
        void CreateEvent(Event ev);
        void UpdateEvent(Guid id, Event ev);
        void DeleteEvent(Guid id);
    }
}
