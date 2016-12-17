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
    public class PersonService
    {
        private readonly PersonRepository _personRepository;
        private readonly RelationshipRepository _relationshipRepository;

        public PersonService(PersonRepository personRepository, RelationshipRepository relationshipRepository)
        {
            _personRepository = personRepository;
            _relationshipRepository = relationshipRepository;
        }

        public IEnumerable<Person> GetAll()
        {
            return _personRepository.GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person);
        }

        public Person ById(int id)
        {
            return _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Relationship> GetRelationships(int id)
        {
            return _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .Include(e => e.PersonRelationships)
                    .ThenInclude(r => r.RelatedPerson)
                .FirstOrDefault(p => p.Id == id)
                .PersonRelationships;
        }

        public Relationship GetRelationshipByRelatedPersonId(int id, int relatedPersonId)
        {
            return _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .Include(e => e.PersonRelationships)
                    .ThenInclude(r => r.RelatedPerson)
                .FirstOrDefault(p => p.Id == id)
                .PersonRelationships
                .FirstOrDefault(pr => pr.RelatedPersonId == relatedPersonId);
        }

        public IEnumerable<Person> GetPersonsFoRelationship(int id)
        {
            var relatedPersonsIdList = _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .Include(e => e.PersonRelationships)
                    .ThenInclude(r => r.RelatedPerson)
                .FirstOrDefault(p => p.Id == id)
                .PersonRelationships.Select(r => r.RelatedPersonId);

            return _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .Include(e => e.PersonRelationships)
                    .ThenInclude(r => r.RelatedPerson)
                .Where(p => !relatedPersonsIdList.Contains(p.Id) && id != p.Id);
        }

        public async Task<bool> Add(Person newPerson)
        {
            _personRepository.Add(newPerson);
            return await _personRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, Person person)
        {
            var oldPerson = _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .FirstOrDefault(p => p.Id == id);

            oldPerson.Name = person.Name;
            oldPerson.Meaning = person.Meaning;
            oldPerson.Start = person.Start;
            oldPerson.End = person.End;
            oldPerson.UnknownStart = person.UnknownStart;
            oldPerson.UnknownEnd = person.UnknownEnd;
            oldPerson.ImageUrl = person.ImageUrl;

            return await _personRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var person = _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .FirstOrDefault(p => p.Id == id);

            var relationships = _relationshipRepository
                .GetAll()
                .Where(r => r.PersonId == id || r.RelatedPersonId == id);

            _relationshipRepository.RemoveRange(relationships);
            _personRepository.Remove(person);

            return await _personRepository.SaveChangesAsync();
        }
    }
}
