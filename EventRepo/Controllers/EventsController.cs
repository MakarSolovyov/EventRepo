using EventRepo.Interfaces;
using EventRepo.Models;
using EventRepo.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventRepo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService) 
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Метод возвращает данные всех по всем событиям
        /// </summary>
        /// <response code="200">Возвращается JSON-структура ApiResult&lt;IReadOnlyList&lt;Event&gt;&gt;
        /// с деталями всех имеющихся в коллекции событий</response>
        [ProducesResponseType(typeof(ApiResult<IReadOnlyList<Event>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet]
        public ApiResult<IReadOnlyList<Event>> GetAllEvents() 
        {
            return new ApiResult<IReadOnlyList<Event>>
            {
                Data = _eventService.GetAllEvents(),
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Получены все имеющиеся события"
            };
        }

        /// <summary>
        /// Метод возвращает данные по событию по указанному идентификатору
        /// </summary>
        /// <param name="id">Идентификатор для получения события</param>
        /// <response code="200">Возвращается JSON-структура ApiResult&lt;Event&gt; с деталями события</response>
        /// <response code="404">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(typeof(ApiBaseResult), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("{id:Guid}")]
        public ApiBaseResult GetEventById(Guid id)
        {
            try
            {
                return new ApiResult<Event>
                {
                    Data = _eventService.GetEventById(id),
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Получено событие с указанным ID"
                };
            }
            catch (ArgumentException exception)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Не удалось найти номер здания по индексу: {exception.Message}"
                };
            }
        }

        /// <summary>
        /// Метод записывает данные по новому событию в коллекцию
        /// </summary>
        /// <param name="eventDTO">Данные по новому событию</param>
        /// <response code="201">Возвращается JSON-структура ApiResult с деталями операции</response>
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status201Created)]
        [Produces("application/json")]
        [HttpPost]
        public ApiResult CreateEvent([FromBody] EventDTO eventDTO)
        {
            var newEvent = new Event(
                eventDTO.Id,
                eventDTO.Title,
                eventDTO.Description,
                eventDTO.StartAt,
                eventDTO.EndAt);

            return new ApiResult
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = $"Добавлено новое событие с ID = {newEvent.Id}"
            };
        }

        /// <summary>
        /// Метод обновляет данные по событию в коллекции
        /// </summary>
        /// <param name="id">Идентификатор события</param>
        /// <param name="eventDTO">Данные по обновленному событию</param>
        /// <response code="204">Возвращается JSON-структура ApiResult с деталями операции</response>
        /// <response code="404">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        [HttpPut("{id:Guid}")]
        public ApiBaseResult UpdateEvent([FromRoute] Guid id, [FromBody] EventDTO eventDTO)
        {
            try
            {
                var newEvent = new Event(
                    eventDTO.Id,
                    eventDTO.Title,
                    eventDTO.Description,
                    eventDTO.StartAt,
                    eventDTO.EndAt);

                _eventService.UpdateEvent(id, newEvent);

                return new ApiResult
                {
                    Success = true,
                    StatusCode = HttpStatusCode.NoContent,
                    Message = "Обновлено событие с указанным ID"
                };
            }
            catch (ArgumentException exception)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Не удалось найти номер здания по индексу: {exception.Message}"
                };
            }
        }

        /// <summary>
        /// Метод удаляет указанное по идентификатору событие из коллекции
        /// </summary>
        /// <param name="id">Идентификатор события</param>
        /// <response code="204">Возвращается JSON-структура ApiResult с деталями операции</response>
        /// <response code="404">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        [HttpDelete("{id:Guid}")]
        public ApiBaseResult DeleteEvent(Guid id)
        {
            try
            {
                _eventService.DeleteEvent(id);

                return new ApiResult
                {
                    Success = true,
                    StatusCode = HttpStatusCode.NoContent,
                    Message = "Удалено событие с указанным ID"
                };
            }
            catch (ArgumentException exception)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Не удалось найти номер здания по индексу: {exception.Message}"
                };
            }
        }
    }
}
