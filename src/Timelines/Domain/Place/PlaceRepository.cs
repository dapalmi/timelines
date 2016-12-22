using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelines.Persistence;

namespace Timelines.Domain.Place
{
    public class PlaceRepository : Repository<Place>
    {
        private TimelinesContext _context;

        public PlaceRepository(TimelinesContext context) : base(context)
        {
            _context = context;
        }
    }
}
