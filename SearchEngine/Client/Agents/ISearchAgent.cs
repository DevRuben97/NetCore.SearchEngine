using SearchEngine.Shared.Entity.Search;
using System.Threading.Tasks;

namespace SearchEngine.Client.Agents
{
    public interface ISearchAgent
    {
        /// <summary>
        /// Search the items based on the supplied query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public Task<SearchResultCollection> ExecuteSearch(string query, int startIndex=0, int endIndex=10);
    }
}
