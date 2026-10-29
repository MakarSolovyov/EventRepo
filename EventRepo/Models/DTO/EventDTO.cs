using System.ComponentModel.DataAnnotations;

namespace EventRepo.Models.DTO
{
    public class EventDTO : IValidatableObject
    {
        [Required(ErrorMessage = "ID события обязателен для заполнения")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Навзвание события обязательно для заполнения")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Время начала события обязательно для заполнения")]
        public DateTime StartAt { get; set; }

        [Required(ErrorMessage = "Время завершения события обязательно для заполнения")]
        public DateTime EndAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartAt >= EndAt)
            {
                yield return new ValidationResult(
                    "Время начала события должно быть раньше времени завершения",
                    new[] { nameof(StartAt), nameof(EndAt) });
            }
        }
    }
}
