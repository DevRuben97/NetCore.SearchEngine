using SearchEngine.Shared.Entity.Search;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SearchEngine.Client.Agents
{
    public class SearchAgent : ISearchAgent
    {

        HttpClient httpClient;

        private string ResourceName;

        public SearchAgent(HttpClient _httpClient)
        {
            this.httpClient = _httpClient;
            this.ResourceName = "/api/Search";
        }

        public async Task<SearchResultCollection> ExecuteSearch(string query, int startIndex = 0, int endIndex = 10)
        {
            var result = await httpClient.GetAsync(ResourceName + $"?query={query}&startIndex={startIndex}&endIndex={endIndex}");

           if (result.StatusCode== System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<SearchResultCollection>();
            }
            else
            {
                return new SearchResultCollection();
            }
        }
    }
}
