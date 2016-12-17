using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timelines.Domain.Person;
using Timelines.Domain.Relationship;
using Timelines.Service;
using Timelines.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Timelines.Controllers.Api
{
    [Route("api/[controller]")]
    public class RelationshipsController : Controller
    {
        private readonly RelationshipService _relationshipService;
        private readonly ILogger<RelationshipsController> _logger;

        public RelationshipsController(RelationshipService relationshipService, ILogger<RelationshipsController> logger)
        {
            _relationshipService = relationshipService;
            _logger = logger;
        }

        [HttpGet("/api/persons/{personId}/relationships")]
        public IActionResult GetRelationships(int personId)
        {
            try
            {
                return Ok(Mapper.Map<IEnumerable<RelationshipViewModel>>(_relationshipService.GetRelationshipsForPerson(personId)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get relationships!", ex);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("/api/persons/{personId}/relationships/{relatedPersonId}")]
        public IActionResult GetRelationshipByRelatedPersonId(int personId, int relatedPersonId)
        {
            try
            {
                return Ok(Mapper.Map<RelationshipViewModel>(_relationshipService.GetRelationshipByRelatedPersonId(personId, relatedPersonId)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get relationship by related person id!", ex);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("/api/relationships/types")]
        public IActionResult GetRelationshipTypes()
        {
            try
            {
                return Ok(Enum.GetNames(typeof(RelationshipType)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get relationship types", ex);
                return BadRequest("Error occurred");
            }
        }

        [HttpPost("/api/persons/{personId}/relationships")]
        public async Task<IActionResult> Post(int personId, [FromBody]RelationshipViewModel relationshipViewModel)
        {
            if (ModelState.IsValid)
            {
                var newRelationship = Mapper.Map<Relationship>(relationshipViewModel);
                if (await _relationshipService.Add(personId, newRelationship))
                {
                    return Created($"api/persons/{newRelationship.PersonId}", Mapper.Map<RelationshipViewModel>(newRelationship));
                }

                _logger.LogError("Failed to save relationship to database");
            }
            else
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogError($"Model not valid ({errorMessage})");
            }

            return BadRequest("Failed to save relationship");
        }

        // PUT api/values/5
        [HttpPut("/api/persons/{personId}/relationships/{relatedPersonId}")]
        public async Task<IActionResult> Put(int personId, int relatedPersonId, [FromBody]RelationshipViewModel relationshipViewModel)
        {
            if (ModelState.IsValid)
            {
                var relationship = Mapper.Map<Relationship>(relationshipViewModel);
                if (await _relationshipService.Update(personId, relatedPersonId, relationship.RelationshipType))
                {
                    return Created($"/api/persons/{personId}/relationships/{relatedPersonId}", Mapper.Map<RelationshipViewModel>(relationship));
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
        [HttpDelete("/api/persons/{personId}/relationships/{relatedPersonId}")]
        public async Task<IActionResult> Delete(int personId, int relatedPersonId)
        {
            if (await _relationshipService.Delete(personId, relatedPersonId))
            {
                return Ok();
            }
            _logger.LogError($"Could not delete realtionship with id {personId} / {relatedPersonId}");
            return BadRequest("Failed to delete relationship");
        }
    }
}
