using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using SearchEngine.Shared.Entity.Search;
using System.Threading.Tasks;
using SearchEngine.Client.Agents;
using Microsoft.AspNetCore.Components.Web;
using SearchEngine.Shared.Enum.Search;
using System;
using Microsoft.JSInterop;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;

namespace SearchEngine.Client.Components.Search
{
    public partial class SearchBox: ComponentBase
    {
        /// <summary>
        /// The resuls of the search:
        /// </summary>
        public SearchResultCollection SearchResults { get; set; }

        public List<SearchResult> SearchHistory { get; set; }

        public bool IsDropdownVisible { get; set; }

        [Inject]
        private ISearchAgent searchAgent { get; set; }

        [Inject]
        private IJSRuntime jSRuntime { get; set; }


        private string HistorySaveKey = "88198233dfd";


        private bool IsLoading = false;

        public string CurrentQuery { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; } = 10;

        public int MinCaracterLength { get; set; } = 4;

        /// <summary>
        /// Icons by type of result.
        /// </summary>
        private Dictionary<SearchableType,string> Icons;


        protected override async Task OnInitializedAsync()
        {
            SearchResults = new SearchResultCollection();
            SearchHistory = new List<SearchResult>();

            //Set the icons
            Icons = new Dictionary<SearchableType,string>();
            Icons.Add(SearchableType.Client, "fas fa-user");
            Icons.Add(SearchableType.Invoices, "fas fa-file");



            await Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           await jSRuntime.InvokeVoidAsync("createSearchBarReference", DotNetObjectReference.Create(this));
        }
        [JSInvokable]
        public async Task ActiveSearchBar()
        {
            await OnGetFocusHandler();
            StateHasChanged();
        }

        public async Task OnGetFocusHandler()
        {
            var history = await jSRuntime.InvokeAsync<List<SearchResult>>("localStorage.getItem",HistorySaveKey);

            if (history!= null && history.Any())
            {
                this.SearchHistory = history;
            }

            IsDropdownVisible = true;
        }

        public void OnLoseFofusHanlder()
        {
            this.IsDropdownVisible = false;
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
                if (SearchResults.Count > 0 || SearchHistory.Count > 0)
                    IsDropdownVisible = true;
                else
                    IsDropdownVisible = false;
            }
            else
            {
                this.SearchResults.Clear();
            }

            StateHasChanged();

        }

        private RenderFragment RenderSuggestions() => builder =>
         {
             if (SearchResults.Count > 0)
             {
                 foreach (var group in SearchResults.GroupBy(x=> x.GetTypeName()))
                 {
                     builder.AddContent(0, RenderItemHeader(group.Key, group.Count()));
                     builder.AddContent(1, b =>
                     {
                         var itemIndex = 0;
                         foreach (var item in group)
                         {
                             b.AddContent(itemIndex, RenderSuggestionItem(item));
                             itemIndex++;
                         }
                     });
                 }
             }

             ////Add the suggestions of history:
             if (SearchHistory.Count> 0)
             {
                 builder.AddContent(2, RenderItemHeader("Search History"));
                 foreach (var history in SearchHistory)
                 {
                     builder.AddContent(0, RenderSuggestionItem(history));
                 }
             }
         };

        private RenderFragment RenderItemHeader(string name,int count=0)=> builder=>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "result_group_header");
            builder.AddContent(2, b =>
            {
                b.OpenElement(0, "h6");
                b.AddContent(1, name);
                b.CloseElement();
                
                if (count > 0)
                {
                    b.OpenElement(2, "h6");
                    b.AddContent(3, $"({count})");
                    b.CloseElement();
                }
                
            });
            builder.CloseElement();
        };

        private async void OnClikcResultHandler(SearchResult result)
        {
            //Save the history of search:
            if (!SearchResults.Contains(result))
            {
                SearchResults.Add(result);
            }

            await jSRuntime.InvokeVoidAsync("localStorage.setItem", HistorySaveKey, this.SearchResults);


            await Task.CompletedTask;
        }

        public RenderFragment RenderSuggestionItem(SearchResult item) => builder =>
         {
             builder.OpenElement(0,"li");
             builder.AddAttribute(1, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => OnClikcResultHandler(item)));
             builder.AddContent(2, b=>
             {
                 b.OpenElement(0, "i");
                 b.AddAttribute(1, "class", GetIconByType(item.GetValue<int>(SearchField.Type)));
                 b.CloseElement();
                 b.AddContent(2, item.GetValue<string>(SearchField.Description));
             });
             builder.CloseElement();
         };

        private string GetIconByType(int v)
        {
            return Icons.GetValueOrDefault((SearchableType)v, string.Empty); 
        }
    }
}
