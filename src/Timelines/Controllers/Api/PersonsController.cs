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
    public class PersonsController : Controller
    {
        private readonly PersonService _personService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(PersonService personService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<PersonViewModel>>(_personService.GetAll()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get all persons!", ex);
                return BadRequest("Error occurred");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(Mapper.Map<PersonViewModel>(_personService.ById(id)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get person with id {id}", ex);
                return BadRequest("Error occurred");
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                var newPerson = Mapper.Map<Person>(personViewModel);
                if (await _personService.Add(newPerson))
                {
                    return Created($"api/events/{personViewModel.Name}", Mapper.Map<PersonViewModel>(newPerson));
                }

                _logger.LogError("Failed to save person to database");
            }
            else
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogError($"Model not valid ({errorMessage})");
            }

            return BadRequest("Failed to save person");
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
