﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        [HttpGet("/api/persons/{personId}/relationships/persons")]
        public IActionResult GetPersonsForRelationship(int personId)
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<PersonViewModel>>(_personService.GetPersonsFoRelationship(personId)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get persons for relationship with id {personId}", ex);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("/api/persons/gendertypes")]
        public IActionResult GetGenderTypes()
        {
            try
            {
                return Ok(Enum.GetNames(typeof(GenderType)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get gender types", ex);
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
                    return Created($"api/persons/{newPerson.Id}", Mapper.Map<PersonViewModel>(newPerson));
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
        public async Task<IActionResult> Put(int id, [FromBody]PersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                var person = Mapper.Map<Person>(personViewModel);
                if (await _personService.Update(id, person))
                {
                    return Created($"api/persons/{person.Id}", Mapper.Map<PersonViewModel>(person));
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

            return BadRequest("Failed to update person");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _personService.Delete(id))
            {
                return Ok();
            }
            _logger.LogError($"Could not delete person with id {id}");
            return BadRequest("Failed to delete person");
        }
    }
}
