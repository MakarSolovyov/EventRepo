using System.Diagnostics.CodeAnalysis;

namespace EventRepo.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public Event(Guid id, string title, string description, DateTime startAt, DateTime endAt) 
        {
            if (startAt > endAt) throw new ArgumentException("Время начала события должно быть раньше времени завершения");

            Id = id; 
            Title = title; 
            Description = description; 
            StartAt = startAt; 
            EndAt = endAt;
        }
    }
}
