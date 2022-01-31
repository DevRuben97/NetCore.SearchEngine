using SearchEngine.Shared.Enum.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using System.ComponentModel.DataAnnotations.Schema;
using SearchEngine.Server.Domain.Interfaces;

namespace SearchEngine.Server.Domain.Base
{
    public abstract class Searchable: ISearchable
    {
        public static readonly Dictionary<SearchField, string> FieldStrings = new Dictionary<SearchField, string>
        {
            {SearchField.Id, "Id" },
            {SearchField.Number, "Number"},
            {SearchField.Description, "Description" },
            {SearchField.Title, "Title" },
            {SearchField.Type, "Type" }
        };

        public static readonly Dictionary<SearchField, string> AnalizedFields = new Dictionary<SearchField, string>
        {
            {SearchField.Title, "Title" },
        };

        public virtual IEnumerable<Field> GetFields()
        {
            return new List<Field>();
        }
    }
}
