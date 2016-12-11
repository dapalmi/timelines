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
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;

        public EventService(EventRepository eventRepository)
        {
            _eventRepository = eventRepository;
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

        public async Task<bool> Add(Event newEvent)
        {
            _eventRepository.Add(newEvent);
            return await _eventRepository.SaveChangesAsync();
        }
    }
}
