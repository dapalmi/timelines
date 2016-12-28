using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timelines.Domain;
using Timelines.Domain.Event;
using Timelines.Domain.Person;
using Timelines.Domain.Place;
using Timelines.Service;
using Timelines.ViewModels;

namespace Timelines.Controllers.Api
{
    [Route("api/[controller]")]
    public class PlacesController : Controller
    {
        private readonly PlaceService _placeService;
        private readonly ILogger<PlacesController> _logger;

        public PlacesController(PlaceService placeService, ILogger<PlacesController> logger)
        {
            _placeService = placeService;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<PlaceViewModel>>(_placeService.GetAll()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get all places!", ex);
                return BadRequest("Error occurred");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(Mapper.Map<PlaceViewModel>(_placeService.ById(id)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get place with id {id}", ex);
                return BadRequest("Error occurred");
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceViewModel placeViewModel)
        {
            if (ModelState.IsValid)
            {
                var newPlace = Mapper.Map<Place>(placeViewModel);
                if (await _placeService.Add(newPlace))
                {
                    return Created($"api/places/{newPlace.Id}", Mapper.Map<PlaceViewModel>(newPlace));
                }

                _logger.LogError("Failed to save place to database");
            }
            else
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogError($"Model not valid ({errorMessage})");
            }

            return BadRequest("Failed to save place");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]PlaceViewModel placeViewModel)
        {
            if (ModelState.IsValid)
            {
                var place = Mapper.Map<Place>(placeViewModel);
                if (await _placeService.Update(id, place))
                {
                    return Created($"api/places/{place.Id}", Mapper.Map<PlaceViewModel>(place));
                }

                _logger.LogError("Failed to save place to database");
            }
            else
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogError($"Model not valid ({errorMessage})");
            }

            return BadRequest("Failed to update place");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _placeService.Delete(id))
            {
                return Ok();
            }
            _logger.LogError($"Could not delete place with id {id}");
            return BadRequest("Failed to delete place");
        }
    }
}
