using System.ComponentModel.DataAnnotations;

namespace EventRepo.Models.DTO
{
    /// <summary>
    /// Модель фильтрации данных событий
    /// </summary>
    public class EventFilterDTO
    {
        /// <summary>
        /// Название (частичное, регистронезависимое)
        /// </summary>
        [StringLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
        public string? Title { get; set; }

        /// <summary>
        /// Время начала события
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        /// Время завершения события
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Номер страницы (начиная с 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Номер страницы должен быть больше или равен 1")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        [Range(1, 100, ErrorMessage = "Размер страницы должен быть в диапазоне 1..100")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Метод валидации введенных данных по фильтрации
        /// </summary>
        /// <param name="validationContext">Контекст валидации,который содержит информацию 
        /// о валидируемом объекте и сервисах, доступных во время валидации</param>
        /// <returns>Результаты валидации: если данные корректны — пустая последовательность,
        /// при ошибке возвращается описание нарушения с указанием затронутых свойств</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.HasValue && To.HasValue)
            {
                if (From >= To)
                {
                    yield return new ValidationResult(
                        "Время начала события должно быть раньше времени завершения",
                        new[] { nameof(From), nameof(To) });
                }
            }
        }
    }
}
