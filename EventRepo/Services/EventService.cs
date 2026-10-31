using EventRepo.Interfaces;
using EventRepo.Models;
using System.Collections.Concurrent;

namespace EventRepo.Services
{
    public class EventService : IEventService
    {
        public static ConcurrentDictionary<Guid, Event> Events { get; set; } = new();

        public IReadOnlyList<Event> GetAllEvents() 
        {
            
            return Events.Values.ToList();
        }

        public Event GetEventById(Guid id) 
        {
            if (!Events.TryGetValue(id, out var ev))
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }

            return ev;
        }

        public void CreateEvent(Event ev) 
        {
            if (!Events.TryAdd(ev.Id, ev))
            {
                throw new ArgumentException($"Событие с ID = {ev.Id} уже существует");
            }
        }

        public void UpdateEvent(Guid id, Event ev) 
        {
            if (!Events.ContainsKey(id))
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }

            Events[id] = ev;
        }

        public void DeleteEvent(Guid id) 
        {
            if (!Events.Remove(id, out _))
            {
                throw new ArgumentException($"Объект с указанным ID = {id} не существует");
            }
        }
    }
}
