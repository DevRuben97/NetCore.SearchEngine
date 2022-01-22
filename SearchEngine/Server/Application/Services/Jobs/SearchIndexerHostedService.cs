using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchEngine.Server.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            timer = new Timer(Execute, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await timer.DisposeAsync();
        }

        public async void Execute(object obj)
        {
            var searchManager = GetSearchManager();
            var unitOfWork = GetUnitOfWork();

            var searchables = typeof(ISearchable).Assembly.ExportedTypes.Where(s => typeof(ISearchable).IsAssignableFrom(s) && !s.IsInterface && !s.IsAbstract)
                .Select(s => ValueTuple.Create(s.GetType(), (ISearchable)Activator.CreateInstance(s)))
                .ToList();


            foreach (var search in searchables)
            {
                var repo = unitOfWork.GetRepository(search.Item1);

                var records = (List<ISearchable>) await repo.ListAsync();

                searchManager.AddToIndex(records.ToArray());
            }
                
            
        }
    }
}
