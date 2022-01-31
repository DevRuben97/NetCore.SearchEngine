using SearchEngine.Shared.Enum.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SearchEngine.Shared.Entity.Search
{
    public class SearchResult
    {
        /// <summary>
        /// The properties of the result:
        /// </summary>
        /// 
        public List<SearchResultValue> Properties { get;  set; }

        public SearchResult()
        {
            Properties = new List<SearchResultValue>();
        }

        public SearchResult(Dictionary<string,string> values)
        {
            Properties = values.Select(x => new SearchResultValue
            {
                Property = x.Key,
                Value = x.Value
            }).ToList();
        }

        /// <summary>
        /// Get the value of the field
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public T GetValue<T>(SearchField field)
        {
            var value = Properties
                .Where(x => x.Property == System.Enum.GetName(typeof(SearchField), (int)field))
                .Select(x => x.Value)
                .FirstOrDefault();


            return (T) Convert.ChangeType(value,typeof(T));
        }

        /// <summary>
        /// Get the type of the result:
        /// </summary>
        /// <returns></returns>
        public string GetTypeName()
        {
            var type = GetValue<int>(SearchField.Type);

            return System.Enum.GetName(typeof(SearchableType),type);
        }


    }
}
