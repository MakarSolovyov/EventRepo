using System.ComponentModel.DataAnnotations;

namespace EventRepo.Models.DTO
{
    /// <summary>
    /// Модель запросов по данным события
    /// </summary>
    public class EventDTO : IValidatableObject
    {
        /// <summary>
        /// Идентификатор события
        /// </summary>
        [Required(ErrorMessage = "ID события обязателен для заполнения")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Наименование события
        /// </summary>
        [Required(ErrorMessage = "Навзвание события обязательно для заполнения")]
        public string Title { get; set; }

        /// <summary>
        /// Описание события
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Время начала события
        /// </summary>
        [Required(ErrorMessage = "Время начала события обязательно для заполнения")]
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Время завершения события
        /// </summary>
        [Required(ErrorMessage = "Время завершения события обязательно для заполнения")]
        public DateTime EndAt { get; set; }

        /// <summary>
        /// Метод валидации введенных данных по событию
        /// </summary>
        /// <param name="validationContext">Контекст валидации,который содержит информацию 
        /// о валидируемом объекте и сервисах, доступных во время валидации</param>
        /// <returns>Результаты валидации: если данные корректны — пустая последовательность,
        /// при ошибке возвращается описание нарушения с указанием затронутых свойств</returns>
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
