using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SearchEngine.Server.Domain.Interfaces;
using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Infraestructure.DataAccess;

namespace SearchEngine.Server.Infraestructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {

        protected AppDbContext dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddAsync(T entity)
        {
           await dbContext.Set<T>().AddAsync(entity);

            return true;
        }

        public async Task<bool> CommitChanges()
        {
            var result = await this.dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
             dbContext.Set<T>().Remove(entity);

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria)
        {
         return await   dbContext.Set<T>().Where(criteria).ToListAsync();
        }
        public async Task<IEnumerable<T>> ListAsync()
        {
            var items=  await dbContext.Set<T>().ToListAsync();
            return items;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;

           return await Task.FromResult(true);
        }


    }
}
