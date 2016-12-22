using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timelines.Domain;
using Timelines.Domain.Person;
using Timelines.Persistence;
using Timelines.Service;
using Timelines.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Timelines.Controllers.Api
{
    [Route("api/[controller]")]
    public class TimelinesController : Controller
    {
        private readonly TimelineService _timelineService;
        private readonly ILogger<TimelineService> _logger;

        public TimelinesController(TimelineService timelineService, ILogger<TimelineService> logger)
        {
            _timelineService = timelineService;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_timelineService.GetAll().OrderBy(t => t.OrderYear));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get all timelines!", ex);
                return BadRequest("Error occurred");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_timelineService.ByPersonId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get person with id {id}", ex);
                return BadRequest("Error occurred");
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
