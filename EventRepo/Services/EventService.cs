using EventRepo.Interfaces;
using EventRepo.Models;

namespace EventRepo.Services
{
    public class EventService : IEventService
    {
        public static List<Event> Events { get; set; } = [];

        public IReadOnlyList<Event> GetAllEvents() 
        {
            
            return Events.AsReadOnly();
        }

        public Event GetEventById(Guid id) 
        {
            var ev = Events.FirstOrDefault(e => e.Id == id);

            if (ev is null)
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }
            else
            {
                return ev;
            }
        }

        public void CreateEvent(Event ev) 
        {
            Events.Add(ev);
        }

        public void UpdateEvent(Guid id, Event ev) 
        {
            var existEvent = Events.FirstOrDefault(e => e.Id == id);
            
            if (existEvent is null)
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }

            var index = Events.IndexOf(existEvent);
            Events[index] = ev;
        }

        public void DeleteEvent(Guid id) 
        {
            var ev = Events.FirstOrDefault(e => e.Id == id);

            if (ev is null)
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }
            else
            {
                Events.Remove(ev);
            }
        }
    }
}
