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
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class PersonService
    {
        private readonly PersonRepository _personRepository;

        public PersonService(PersonRepository personRepository)
        {
            _personRepository = personRepository;
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

        public async Task<bool> Add(Person newPerson)
        {
            _personRepository.Add(newPerson);
            return await _personRepository.SaveChangesAsync();
        }
    }
}
