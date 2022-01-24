using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SearchEngine.Server.Application.Services.Jobs
{
    public class SearchIndexerHostedService : IHostedService
    {
        private Timer timer;

        IServiceProvider serviceProvider;

        private static ISearchManager _searchManager;

        private IUnitOfWork _unitOfWork;

        public SearchIndexerHostedService(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }

        private ISearchManager GetSearchManager()
        {
            if (_searchManager != null)
            {
                return _searchManager;
            }

            using var scope = serviceProvider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<ISearchManager>();

            return _searchManager = manager;

        }

        private IUnitOfWork GetUnitOfWork()
        {
            if(_unitOfWork!= null)
            {
                return _unitOfWork;
            }

            using var scope = serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            return _unitOfWork = unitOfWork;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Execute, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(5));

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await timer.DisposeAsync();
        }

        //private List<ISearchable> GetSearchables(object item, Type type)
        //{
        //   var json= JsonSerializer.Serialize(item);

        //   var items= JsonSerializer.Deserialize(json, typeof(List<object>));
        //}

        public async void Execute(object obj)
        {
            var searchManager = GetSearchManager();
            var unitOfWork = GetUnitOfWork();

            //Get all Search entities of the assembly:
            var searchables = typeof(ISearchable).Assembly.ExportedTypes.Where(s => typeof(ISearchable).IsAssignableFrom(s) && !s.IsInterface && !s.IsAbstract)
                .Select(s => s)
                .ToList();


            //Iterate all data for add to the index
            foreach (var searchable in searchables)
            {
                var method = typeof(IUnitOfWork).GetMethod(nameof(IUnitOfWork.GetRepository));

                var methodType = method.MakeGenericMethod(searchable);

                var repo = methodType.Invoke(unitOfWork, null);

                var listMethod = repo.GetType().GetMethod(nameof(IBaseRepository<Entity>.ListAsync));

                Task task = (Task) listMethod.Invoke(repo, null);

                await task.ConfigureAwait(false);

                var result = task.GetType().GetProperty("Result")
                    .GetValue(task, null);

                var items= (IEnumerable)result;

                //searchManager.AddToIndex(items.Select(x => (ISearchable)x).ToArray());
            }
                
            
        }
    }
}
