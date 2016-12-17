using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Timelines.Domain.Event;
using Timelines.Domain.Person;
using Timelines.ViewModels;

namespace Timelines.Automapper
{
    public class TimelineEventsCustomResolver : ValueResolver<Person, IEnumerable<EventViewModel>>
    {
        protected override IEnumerable<EventViewModel> ResolveCore(Person source)
        {
            var events = source.PersonEvents.Select(pe => Mapper.Map<EventViewModel>(pe.Event)).OrderBy(e => e.Year).ToArray();
            
            for (int i = 0; i < events.Count(); i++)
            {
                if (i > 0)
                {
                    events[i].PreviousEventYear = events[i - 1].Year;
                }
            }

            return events;
        }
    }
}
