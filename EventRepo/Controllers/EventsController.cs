using EventRepo.Interfaces;
using EventRepo.Models;
using EventRepo.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public ActionResult<ApiResult<IReadOnlyList<Event>>> GetAllEvents() 
        {
            var events = _eventService.GetAllEvents();

            return Ok(new ApiResult<IReadOnlyList<Event>>
            {
                Data = events,
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = $"Получены все имеющиеся события, кол-во событий: {events.Count}"
            });
        }

        /// <summary>
        /// Метод возвращает данные по событию по указанному идентификатору
        /// </summary>
        /// <param name="id">Идентификатор для получения события</param>
        /// <response code="200">Возвращается JSON-структура ApiResult&lt;Event&gt; с деталями события</response>
        /// <response code="404">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(typeof(ApiResult<Event>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpGet("{id:Guid}")]
        public ActionResult<ApiBaseResult> GetEventById(Guid id)
        {
            try
            {
                return Ok(new ApiResult<Event>
                {
                    Data = _eventService.GetEventById(id),
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Получено событие с указанным ID"
                });
            }
            catch (ArgumentException exception)
            {
                return NotFound(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Не удалось найти событие по идентификатору: {exception.Message}"
                });
            }
        }

        /// <summary>
        /// Метод записывает данные по новому событию в коллекцию
        /// </summary>
        /// <param name="eventDTO">Данные по новому событию</param>
        /// <response code="201">Возвращается JSON-структура ApiResult с деталями операции</response>
        /// <response code="400">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        /// <response code="409">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        [HttpPost]
        public ActionResult<ApiResult> CreateEvent([FromBody] EventDTO eventDTO)
        {
            try
            {
                var newEvent = new Event(
                eventDTO.Id,
                eventDTO.Title,
                eventDTO.Description,
                eventDTO.StartAt.Value,
                eventDTO.EndAt.Value);

                _eventService.CreateEvent(newEvent);

                return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, new ApiResult
                {
                    Success = true,
                    StatusCode = HttpStatusCode.Created,
                    Message = $"Добавлено новое событие с ID = {newEvent.Id}"
                });
            }
            catch (ArgumentException exception)
            {
                return Conflict(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.Conflict,
                    Message = $"Событие по указанному идентификатору уже существует: {exception.Message}"
                });
            }
        }

        /// <summary>
        /// Метод обновляет данные по событию в коллекции
        /// </summary>
        /// <param name="id">Идентификатор события</param>
        /// <param name="eventDTO">Данные по обновленному событию</param>
        /// <response code="204">HTTP статус код без контента</response>
        /// <response code="400">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        /// <response code="404">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpPut("{id:Guid}")]
        public ActionResult<ApiBaseResult> UpdateEvent([FromRoute] Guid id, [FromBody] EventDTO eventDTO)
        {
            try
            {
                var newEvent = new Event(
                    eventDTO.Id,
                    eventDTO.Title,
                    eventDTO.Description,
                    eventDTO.StartAt.Value,
                    eventDTO.EndAt.Value);

                if (eventDTO.Id != id)
                {
                    return BadRequest(new ApiResult
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "ID в маршруте и в теле запроса не совпадают"
                    });
                }

                _eventService.UpdateEvent(id, newEvent);

                return NoContent();
            }
            catch (ArgumentException exception)
            {
                return NotFound(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Не удалось найти событие по идентификатору: {exception.Message}"
                });
            }
        }

        /// <summary>
        /// Метод удаляет указанное по идентификатору событие из коллекции
        /// </summary>
        /// <param name="id">Идентификатор события</param>
        /// <response code="204">HTTP статус код без контента</response>
        /// <response code="404">Возвращается JSON-структура ApiResult с деталями ошибки</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpDelete("{id:Guid}")]
        public ActionResult<ApiBaseResult> DeleteEvent(Guid id)
        {
            try
            {
                _eventService.DeleteEvent(id);

                return NoContent();
            }
            catch (ArgumentException exception)
            {
                return NotFound(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Не удалось найти событие по идентификатору: {exception.Message}"
                });
            }
        }
    }
}
