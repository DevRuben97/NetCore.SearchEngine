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
        private Dictionary<string,object> _properties;

        public SearchResult(Dictionary<string,object> values)
        {
            
        }

        /// <summary>
        /// Get the value of the field
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object GetValue<T>(SearchField field) where T : class
        {
            var value = _properties
                .Where(x=> x.Key== System.Enum.GetName(typeof(SearchField), (int) field))
                .FirstOrDefault();

            return Convert.ChangeType(value,typeof(T));
        }



    }
}
