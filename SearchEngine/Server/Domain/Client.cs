using Lucene.Net.Documents;
using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Domain.Interfaces;
using SearchEngine.Shared.Enum.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain
{
    public class Client: Entity, ISearchable
    {
        public string Name { get; set; }

        public  string SurName { get; set; }

        public string Email { get; set; }

        public IEnumerable<Field> GetFields()
        {
            var title = $"{Name} {SurName}";
            return new Field[]
           {
              
               new TextField(Searchable.AnalizedFields[SearchField.Description], title, Field.Store.NO),
               new TextField(Searchable.AnalizedFields[SearchField.Title], title, Field.Store.YES){Boost= 4.0f},
               new StringField(Searchable.AnalizedFields[SearchField.Id], Id.ToString(), Field.Store.YES),
               new StringField(Searchable.AnalizedFields[SearchField.Number], Id.ToString(), Field.Store.YES)
           };
        }
    }
}
