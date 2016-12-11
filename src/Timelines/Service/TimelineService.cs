using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelines.Domain.Event;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;
using Remotion.Linq.Clauses;
using Timelines.Domain.Person;
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class TimelineService
    {
        private readonly PersonRepository _personRepository;

        public TimelineService(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IEnumerable<TimelineViewModel> GetAll()
        {
            var allPersons = _personRepository.GetAll()
                .Include(p => p.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .Include(p => p.PersonEvents)
                    .ThenInclude(pe => pe.Event)
                .Include(p => p.RelatedPersonRelationships);
            var timelines = allPersons.Select(p => Mapper.Map<TimelineViewModel>(p));
            return timelines;
        }

        public TimelineViewModel ByPersonId(int personId)
        {
            var person = _personRepository.GetAll()
                .Where(p => p.Id == personId)
                .Include(p => p.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .Include(p => p.PersonEvents)
                    .ThenInclude(pe => pe.Event)
                .FirstOrDefault();
            return Mapper.Map<TimelineViewModel>(person);
        }
    }
}
