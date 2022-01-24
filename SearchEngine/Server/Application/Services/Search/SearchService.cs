using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Domain.Interfaces;
using SearchEngine.Shared.Entity.Search;
using System.Linq;

namespace SearchEngine.Server.Application.Services.Search
{
    public class SearchService : ISearchService
    {

        private readonly ISearchManager _searchManager;

        public SearchService(ISearchManager searchManager)
        {
            this._searchManager = searchManager;
        }

        public SearchResultCollection Search(string query, int startIndex, int endIndex)
        {
            return this._searchManager.Search(query, startIndex, endIndex, Searchable.FieldStrings.Values.ToArray());
        }
    }
}
