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

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var newEvent = Mapper.Map<Event>(eventViewModel);
                if (await _eventService.Add(newEvent))
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

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
