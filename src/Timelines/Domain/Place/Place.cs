using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelines.Domain.Event;

namespace Timelines.Domain.Place
{
    public class Place : Entity
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public List<Event.Event> Events { get; set; }
    }
}
