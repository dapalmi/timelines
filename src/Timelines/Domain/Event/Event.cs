using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelines.Domain.Person;

namespace Timelines.Domain.Event
{
    public class Event : Entity
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }

        public List<Person.PersonEvent> PersonEvents { get; set; } = new List<PersonEvent>();
    }
}
