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

        [HttpPost]
        public ApiBaseResult CreateEvent([FromBody] EventDTO eventDTO)
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
