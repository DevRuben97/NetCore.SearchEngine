using SearchEngine.Shared.Entity.Search;

namespace SearchEngine.Server.Application.Services.Search
{
    public interface ISearchService
    {
        /// <summary>
        /// Get the result based on the query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public SearchResultCollection Search(string query, int startIndex, int endIndex);
    }
}
