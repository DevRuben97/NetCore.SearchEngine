using AutoMapper;
using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Application.Services
{
    public class BaseService<T, D> : IBaseService<T, D> where T : Entity
    {
        readonly IUnitOfWork unitOfWork;

        readonly IMapper mapper;

        public BaseService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task AddAsync(D item)
        {
            var repo = unitOfWork.GetRepository<T>();

            await repo.AddAsync(mapper.Map<T>(item));

            await repo.CommitChanges();
        }

        public async Task DeleteAsync(D item)
        {
            var repo = unitOfWork.GetRepository<T>();

            await repo.DeleteAsync(mapper.Map<T>(item));

            await repo.CommitChanges();
        }

        public async Task<List<D>> GetAllAsync()
        {
            var repo = unitOfWork.GetRepository<T>();

            var result= (await repo.ListAsync()).ToList();

            return mapper.Map<List<D>>(result);

        }

        public async Task<D> GetByIdAsync(int id)
        {
            var repo = unitOfWork.GetRepository<T>();

            var result = (await repo.FindAsync(s => s.Id == id)).FirstOrDefault();

            return mapper.Map<D>(result);
        }

        public async Task UpdateAsync(D item)
        {
            var repo = unitOfWork.GetRepository<T>();

           await repo.AddAsync(mapper.Map<T>(item));

            await repo.CommitChanges();
        }
    }
}
