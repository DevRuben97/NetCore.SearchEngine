using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Shared.Entity.Search
{
    /// <summary>
    /// Entity used for the result of the query
    /// </summary>
    public class Search
    {
        public SearchResultCollection Results { get; set; }

        public string SearchQuery { get; set; }
    }
}
