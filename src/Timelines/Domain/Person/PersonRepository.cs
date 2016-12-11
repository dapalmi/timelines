using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelines.Persistence;

namespace Timelines.Domain.Person
{
    public class PersonRepository : Repository<Person>
    {
        private TimelinesContext _context;

        public PersonRepository(TimelinesContext context) : base(context)
        {
            _context = context;
        }
    }
}
