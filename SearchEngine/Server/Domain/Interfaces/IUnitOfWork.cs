using SearchEngine.Server.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Interfaces
{
   public interface IUnitOfWork
    {

        IBaseRepository<T> GetRepository<T>() where T : Entity;

         Task<int> CommitChanges();
    }
}
