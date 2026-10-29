namespace EventRepo.Models
{
    public class Event
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public required DateTime StartAt { get; set; }
        public required DateTime EndAt { get; set; }

        public Event(int id, string title, string description, DateTime startAt, DateTime endAt) 
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
