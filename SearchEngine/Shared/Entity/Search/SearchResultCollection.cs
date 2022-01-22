using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Shared.Entity.Search
{
  public  class SearchResultCollection: ICollection<SearchResult>
    {
        private List<SearchResult> _data { get; set; }

        public int Count { 
            get { return _data.Count; }
            private set { Count = value; }
        }

        public bool IsReadOnly => throw new NotImplementedException();

        public SearchResultCollection()
        {
            _data = new List<SearchResult>();
        }

        public SearchResultCollection(List<SearchResult> data)
        {
            _data = data;
        }

        public void Add(SearchResult item)
        {
            this._data.Add(item);
        }

        public void Clear()
        {
            this._data.Clear();
        }

        public bool Contains(SearchResult item)
        {
           return  _data.Contains(item);
        }

        public void CopyTo(SearchResult[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(SearchResult item)
        {
          return  _data.Remove(item);
        }

        public IEnumerator<SearchResult> GetEnumerator()
        {
          return  _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
