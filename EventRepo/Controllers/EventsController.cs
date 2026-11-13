using EventRepo.Interfaces;
using EventRepo.Models;
using EventRepo.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventRepo.Controllers
{
    [Route("events")]
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
            var ev = _eventService.GetEventById(id);

            if (ev is null)
                return NotFound(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Объект с указанным ID = {id} не существует"
                });

            return Ok(new ApiResult<Event>
            {
                Data = ev,
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Получено событие с указанным ID"
            });
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
            var newEventGuid = _eventService.CreateEvent(eventDTO);

            if (newEventGuid is null)
                return Conflict(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.Conflict,
                    Message = $"Событие с указанным ID уже существует"
                });

            return CreatedAtAction(nameof(GetEventById), new { id = newEventGuid }, new ApiResult
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = $"Добавлено новое событие"
            });
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
            if (!_eventService.UpdateEvent(id, eventDTO))
                return NotFound(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Объект с указанным ID = {id} не существует"
                });

            return NoContent();
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
            if (!_eventService.DeleteEvent(id))
            {
                return NotFound(new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Объект с указанным ID = {id} не существует"
                });
            }

            return NoContent();
        }
    }
}
