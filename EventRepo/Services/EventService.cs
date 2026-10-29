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

        public Event GetEvent(int id) 
        {
            var ev = Events.FirstOrDefault(e => e.Id == id);

            if (ev is null)
            {
                throw new ArgumentException("Объект с указанным id не существует");
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

        public void UpdateEvent(int id, Event ev) 
        {
            var existEvent = Events.FirstOrDefault(e => e.Id == id);
            
            if (existEvent is null)
            {
                throw new ArgumentException("Объект с указанным id не существует");
            }

            var index = Events.IndexOf(existEvent);
            Events[index] = ev;
        }

        public void DeleteEvent(int id) 
        {
            var ev = Events.FirstOrDefault(e => e.Id == id);

            if (ev is null)
            {
                throw new ArgumentException("Объект с указанным id не существует");
            }
            else
            {
                Events.Remove(ev);
            }
        }
    }
}
