using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Domain.Interfaces;
using SearchEngine.Server.Infraestructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Infraestructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

       private readonly AppDbContext dbContext;

        private readonly Dictionary<Type, object> Repositories;

        public UnitOfWork(AppDbContext _appDbContext)
        {
            dbContext = _appDbContext;
            Repositories = new Dictionary<Type, object>();
        }

        public async Task<int> CommitChanges()
        {
           return await this.dbContext.SaveChangesAsync();
        }

        public IBaseRepository<T> GetRepository<T>() where T : Entity
        {
            if (this.Repositories.ContainsKey(typeof(T)))
            {
                return (IBaseRepository<T>)this.Repositories[typeof(T)];
            }

            var repo = new BaseRepository<T>(this.dbContext);
            this.Repositories.Add(typeof(T), repo);

            return repo;
        }
    }
}
