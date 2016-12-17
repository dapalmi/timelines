using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelines.Persistence;

namespace Timelines.Domain.Relationship
{
    public class RelationshipRepository
    {
        private readonly TimelinesContext _context;

        public RelationshipRepository(TimelinesContext context)
        {
            _context = context;
        }

        public IQueryable<Relationship> GetAll()
        {
            return _context.Relationships.AsQueryable();
        }

        public void Add(Relationship relationship)
        {
            _context.Relationships.Add(relationship);
        }

        public void Remove(Relationship relationship)
        {
            _context.Relationships.Remove(relationship);
        }

        public void RemoveRange(IEnumerable<Relationship> relationships)
        {
            _context.Relationships.RemoveRange(relationships);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
