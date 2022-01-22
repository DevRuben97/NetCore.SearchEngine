using SearchEngine.Server.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<IEnumerable<T>> ListAsync();

        Task<bool> DeleteAsync(T Entity);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria);

        Task<bool> CommitChanges();

    }
}
