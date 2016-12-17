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
using Microsoft.CodeAnalysis.CSharp;
using Timelines.Domain.Person;
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly PersonRepository _personRepository;

        public EventService(EventRepository eventRepository, PersonRepository personRepository)
        {
            _eventRepository = eventRepository;
            _personRepository = personRepository;
        }

        public IEnumerable<Event> GetAll()
        {
            return _eventRepository.GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person);
        }

        public Event ById(int id)
        {
            return _eventRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person)
                .FirstOrDefault(e => e.Id == id);
        }
        public IEnumerable<Event> GetByPersonId(int personId)
        {
            return _eventRepository.GetAll()
                .Where(e => e.PersonEvents.Any(pe => pe.PersonId == personId))
                .Include(e => e.PersonEvents)
                    .ThenInclude(pe => pe.Person);
        }

        public async Task<bool> Add(int personId, Event newEvent)
        {
            _eventRepository.Add(newEvent);
            var person = _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                .FirstOrDefault(p => p.Id == personId);

            var newPersonEvent = new PersonEvent()
            {
                Person = person,
                Event = newEvent
            };

            person.PersonEvents.Add(newPersonEvent);

            return await _eventRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, Event ev)
        {
            var oldEvent = _eventRepository
                .GetAll()
                .FirstOrDefault(e => e.Id == id);

            oldEvent.Name = ev.Name;
            oldEvent.Year = ev.Year;
            oldEvent.Text = ev.Text;
            oldEvent.ImageUrl = ev.ImageUrl;

            return await _eventRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var ev = _eventRepository
                .GetAll()
                .FirstOrDefault(e => e.Id == id);
            _eventRepository.Remove(ev);
            return await _eventRepository.SaveChangesAsync();
        }
    }
}
