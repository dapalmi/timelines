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
using Timelines.Domain.Place;
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly PersonRepository _personRepository;
        private readonly PlaceRepository _placeRepository;

        public EventService(EventRepository eventRepository, PersonRepository personRepository,
            PlaceRepository placeRepository)
        {
            _eventRepository = eventRepository;
            _personRepository = personRepository;
            _placeRepository = placeRepository;
        }

        public IEnumerable<Event> GetAll()
        {
            return _eventRepository.GetAll()
                .Include(e => e.PersonEvents)
                .ThenInclude(pe => pe.Person)
                .Include(e => e.Place);
        }

        public Event ById(int id)
        {
            return _eventRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                .ThenInclude(pe => pe.Person)
                .Include(e => e.Place)
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Event> GetByPersonId(int personId)
        {
            return _eventRepository.GetAll()
                .Where(e => e.PersonEvents.Any(pe => pe.PersonId == personId))
                .Include(e => e.PersonEvents)
                .ThenInclude(pe => pe.Person)
                .Include(e => e.Place);
        }

        public async Task<bool> Add(int personId, Event newEvent)
        {
            if (newEvent.Place?.Id != null)
            {
                newEvent.PlaceId = newEvent.Place?.Id;
                newEvent.Place = null;
            }

            if (newEvent.Id == 0)
            {
                _eventRepository.Add(newEvent);
            }

            var newPersonEvent = new PersonEvent
            {
                PersonId = personId,
                Event = newEvent
            };

            var person = _personRepository
                .GetAll()
                .Include(e => e.PersonEvents)
                .FirstOrDefault(p => p.Id == personId);

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
            if (ev.Place?.Id != null)
            {
                oldEvent.PlaceId = ev.Place.Id;
            }

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
