using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using SearchEngine.Shared.Entity.Search;
using System.Threading.Tasks;
using SearchEngine.Client.Agents;
using Microsoft.AspNetCore.Components.Web;
using SearchEngine.Shared.Enum.Search;

namespace SearchEngine.Client.Components.Search
{
    public partial class SearchBox: ComponentBase
    {
        /// <summary>
        /// The resuls of the search:
        /// </summary>
        public SearchResultCollection SearchResults { get; set; } = new SearchResultCollection();

        [Inject]
        private ISearchAgent searchAgent { get; set; }

        private bool IsLoading = false;

        public string CurrentQuery { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; } = 10;


        public int MinCaracterLength { get; set; } = 3;

        protected override async Task OnInitializedAsync()
        {

        }

        private async Task<SearchResultCollection> SearchResultsAsync()
        {
            return await searchAgent.ExecuteSearch(CurrentQuery, StartIndex, EndIndex);
        }

        private async void OnChangeInputValueHanlder(string newQuery)
        {
            this.CurrentQuery = newQuery;

            if (this.CurrentQuery.Length > MinCaracterLength)
            {

                //Search the resuls
                IsLoading = true;
                this.SearchResults = await SearchResultsAsync();
                IsLoading = false;
            }

        }

        private RenderFragment RenderSuggestions() => builder =>
         {
             if (SearchResults.Count > 0)
             {
                 foreach (var item in SearchResults)
                 {
                     builder.AddContent(0, RenderSuggestionItem(item));
                 }
             }
         };

        public RenderFragment RenderSuggestionItem(SearchResult item) => builder =>
         {
             builder.OpenElement(0,"li");
             builder.AddContent(1, item.GetValue<string>(SearchField.Description));
             builder.CloseElement();
         };

    }
}
