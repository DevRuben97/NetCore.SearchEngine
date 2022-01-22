using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Shared.Entity.Search
{
   public class SearchResult
    {
        private readonly Document _Doc;

        public SearchResult(Document document)
        {
            _Doc = document;
        }

    }
}
