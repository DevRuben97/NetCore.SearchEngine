using SearchEngine.Server.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Application.Services
{
    public interface IBaseService<T,D> where T: Entity
    {
        Task AddAsync(D item);

        Task UpdateAsync(D item);

        Task DeleteAsync(D item);

        Task<D> GetByIdAsync(int id);

        Task<List<D>> GetAllAsync();
    }
}
