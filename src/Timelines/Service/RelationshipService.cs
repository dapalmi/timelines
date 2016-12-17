using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelines.Domain.Event;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;
using Timelines.Domain.Person;
using Timelines.Domain.Relationship;
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class RelationshipService
    {
        private readonly RelationshipRepository _relationshipRepository;
        private readonly PersonRepository _personRepository;

        public RelationshipService(RelationshipRepository relationshipRepository, PersonRepository personRepository)
        {
            _relationshipRepository = relationshipRepository;
            _personRepository = personRepository;
        }

        public IEnumerable<Relationship> GetRelationshipsForPerson(int personId)
        {
            return _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                .ThenInclude(pe => pe.Person)
                .Include(e => e.PersonRelationships)
                .ThenInclude(pr => pr.RelatedPerson)
                .FirstOrDefault(p => p.Id == personId)
                .PersonRelationships;
        }

        public Relationship GetRelationshipByRelatedPersonId(int personId, int relatedPersonId)
        {
            return _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                .ThenInclude(pe => pe.Person)
                .Include(e => e.PersonRelationships)
                .ThenInclude(pr => pr.RelatedPerson)
                .FirstOrDefault(p => p.Id == personId)
                .PersonRelationships
                .FirstOrDefault(pr => pr.RelatedPersonId == relatedPersonId);
        }

        public async Task<bool> Add(int personId, Relationship relationship)
        {
            var newRelationship = new Relationship
            {
                PersonId = personId,
                RelatedPersonId = relationship.RelatedPerson.Id,
                RelationshipType = relationship.RelationshipType
            };

            var reverseRelationshipType = newRelationship.RelationshipType;
            if (reverseRelationshipType == RelationshipType.Parent)
            {
                reverseRelationshipType = RelationshipType.Child;
            }
            else if (reverseRelationshipType == RelationshipType.Child)
            {
                reverseRelationshipType = RelationshipType.Parent;
            }

            var reverseRelationship = new Relationship()
            {
                PersonId = newRelationship.RelatedPersonId,
                RelatedPersonId = newRelationship.PersonId,
                RelationshipType = reverseRelationshipType
            };

            _relationshipRepository.Add(newRelationship);
            _relationshipRepository.Add(reverseRelationship);

            return await _relationshipRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(int personId, int relatedPersonId, RelationshipType relationshipType)
        {
            var oldRelationship = _relationshipRepository
                .GetAll()
                .FirstOrDefault(p => p.PersonId == personId && p.RelatedPersonId == relatedPersonId);

            var oldReverseRelationship = _relationshipRepository
                .GetAll()
                .FirstOrDefault(p => p.PersonId == relatedPersonId && p.RelatedPersonId == personId);

            oldRelationship.RelationshipType = relationshipType;

            if (relationshipType == RelationshipType.Parent)
            {
                relationshipType = RelationshipType.Child;
            }
            else if (relationshipType == RelationshipType.Child)
            {
                 relationshipType = RelationshipType.Parent;
            }

            oldReverseRelationship.RelationshipType = relationshipType;

            return await _relationshipRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int personId, int relatedPersonId)
        {
            var relationship = _relationshipRepository
                .GetAll()
                .FirstOrDefault(r => r.PersonId == personId && r.RelatedPersonId == relatedPersonId);

            var reversRelationship = _relationshipRepository
                .GetAll()
                .FirstOrDefault(r => r.PersonId == relatedPersonId && r.RelatedPersonId == personId);

            _relationshipRepository.Remove(relationship);
            _relationshipRepository.Remove(reversRelationship);

            return await _relationshipRepository.SaveChangesAsync();
        }
    }
}
