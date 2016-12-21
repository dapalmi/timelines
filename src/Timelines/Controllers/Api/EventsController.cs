using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timelines.Domain;
using Timelines.Domain.Event;
using Timelines.Service;
using Timelines.ViewModels;

namespace Timelines.Controllers.Api
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly EventService _eventService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(EventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<EventViewModel>>(_eventService.GetAll()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get all events!", ex);
                return BadRequest("Error occurred");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(Mapper.Map<EventViewModel>(_eventService.ById(id)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get event with id {id}", ex);
                return BadRequest("Error occurred");
            }
        }

        // GET: api/values
        [HttpGet("/api/persons/{personId}/events")]
        public IActionResult GetByPersonId(int personId)
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<EventViewModel>>(_eventService.GetByPersonId(personId)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get events by person id!", ex);
                return BadRequest("Error occurred");
            }
        }

        // POST api/values
        [HttpPost("/api/persons/{personId}/events")]
        public async Task<IActionResult> Post(int personId, [FromBody]EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var newEvent = Mapper.Map<Event>(eventViewModel);
                if (await _eventService.Add(personId, newEvent))
                {
                    return Created($"api/events/{eventViewModel.Name}", Mapper.Map<EventViewModel>(newEvent));
                }

                _logger.LogError("Failed to save event to database");
            }
            else
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogError($"Model not valid ({errorMessage})");
            }

            return BadRequest("Failed to save event");
        }

        // POST api/values
        [HttpPost("/api/persons/{personId}/events/{eventId}")]
        public async Task<IActionResult> Post(int personId, int eventId)
        {
            var newEvent = _eventService.ById(eventId);
            if (await _eventService.Add(personId, newEvent))
            {
                return Created($"api/events/{newEvent.Name}", Mapper.Map<EventViewModel>(newEvent));
            }

            _logger.LogError("Failed to save event to database");

            var errorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            _logger.LogError($"Model not valid ({errorMessage})");

            return BadRequest("Failed to save event");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var ev = Mapper.Map<Event>(eventViewModel);
                if (await _eventService.Update(id, ev))
                {
                    return Created($"api/events/{ev.Id}", Mapper.Map<EventViewModel>(ev));
                }

                _logger.LogError("Failed to save event to database");
            }
            else
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogError($"Model not valid ({errorMessage})");
            }

            return BadRequest("Failed to update event");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _eventService.Delete(id))
            {
                return Ok();
            }
            _logger.LogError($"Could not delete event with id {id}");
            return BadRequest("Failed to event person");
        }
    }
}
