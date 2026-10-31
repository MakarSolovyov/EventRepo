using EventRepo.Interfaces;
using EventRepo.Models;
using System.Collections.Concurrent;

namespace EventRepo.Services
{
    public class EventService : IEventService
    {
        private readonly ConcurrentDictionary<Guid, Event> _events = new();

        public IReadOnlyList<Event> GetAllEvents() 
        {
            
            return _events.Values.ToList();
        }

        public Event GetEventById(Guid id) 
        {
            if (!_events.TryGetValue(id, out var ev))
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }

            return ev;
        }

        public void CreateEvent(Event ev) 
        {
            if (!_events.TryAdd(ev.Id, ev))
            {
                throw new ArgumentException($"Событие с ID = {ev.Id} уже существует");
            }
        }

        public void UpdateEvent(Guid id, Event ev) 
        {
            if (!_events.ContainsKey(id))
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }

            _events[id] = ev;
        }

        public void DeleteEvent(Guid id) 
        {
            if (!_events.Remove(id, out _))
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }
        }
    }
}
