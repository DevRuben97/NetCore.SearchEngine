using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Shared.Entity.Search
{
    public class SearchResultValue
    {
        public string Property { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Property} - {Value}";
        }
    }
}
