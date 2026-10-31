using System.Diagnostics.CodeAnalysis;

namespace EventRepo.Models
{
    /// <summary>
    /// Класс, содержащий информацию по событию
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Идентификатор события
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование события
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание события
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Время начала события
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Время завершения события
        /// </summary>
        public DateTime EndAt { get; set; }

        /// <summary>
        /// Конструктор для создания экземпляра класса Event
        /// </summary>
        /// <param name="id">Идентификатор события</param>
        /// <param name="title">Наименование события</param>
        /// <param name="description">Описание события</param>
        /// <param name="startAt">Время начала события</param>
        /// <param name="endAt">Время завершения события</param>
        // /// <exception cref="ArgumentException">Исключение при неправильном указании времени события</exception>
        public Event(Guid id, string title, string description, DateTime startAt, DateTime endAt) 
        {
            //if (startAt > endAt) throw new ArgumentException("Время начала события должно быть раньше времени завершения");

            Id = id; 
            Title = title; 
            Description = description; 
            StartAt = startAt; 
            EndAt = endAt;
        }
    }
}
