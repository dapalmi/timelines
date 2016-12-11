using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelines.Domain
{
    public interface IReadonlyRepository<out T>
    {
        IQueryable<T> GetAll();
    }
}
