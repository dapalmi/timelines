using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timelines.Domain.Person
{
    public class PersonEvent
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int EventId { get; set; }
        public Event.Event Event { get; set; }
    }
}
