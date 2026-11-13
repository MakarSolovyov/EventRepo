namespace EventRepo.Models.DTO
{
    /// <summary>
    /// Модель запросов событий с использованием пагинации
    /// </summary>
    /// <typeparam name="T">Тип элемента коллекции</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Общее количество событий во всей коллекции
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// События текущей страницы
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();

        /// <summary>
        /// Номер текущей страницы (начиная с 1)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Количество элементов на текущей странице
        /// </summary>
        public int PageSize { get; set; }
    }
}
