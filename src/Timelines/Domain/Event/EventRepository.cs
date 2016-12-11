using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using Timelines.Persistence;

namespace Timelines.Domain.Event
{
    public class EventRepository : Repository<Event>
    {
        private TimelinesContext _context;

        public EventRepository(TimelinesContext context) : base(context)
        {
            _context = context;
        }
    }
}
