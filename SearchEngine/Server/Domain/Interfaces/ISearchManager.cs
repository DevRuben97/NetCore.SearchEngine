using SearchEngine.Server.Domain.Base;
using SearchEngine.Shared.Entity.Search;
using SearchEngine.Shared.Enum.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Interfaces
{
  public  interface ISearchManager
    {
        void AddToIndex(params ISearchable[] searchables);

        void DeleteFromIndex(params ISearchable[] searchables);

        void Clear();

        SearchResultCollection Search(string query, int hitsStart, int hitStop, string[] fields);
    }
}
