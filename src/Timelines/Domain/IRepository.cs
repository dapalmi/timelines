using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelines.Domain
{
    public interface IRepository<T> : IReadonlyRepository<T>
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);

        void Edit(T entity);

        void Save();

        Task<bool> SaveChangesAsync();
    }
}
