using EventRepo.Models;

namespace EventRepo.Interfaces
{
    public interface IEventService
    {
        IReadOnlyList<Event> GetAllEvents();
        Event GetEvent(int id);
        void CreateEvent(Event ev);
        void UpdateEvent(int id, Event ev);
        void DeleteEvent(int id);
    }
}
