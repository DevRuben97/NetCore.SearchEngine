using SearchEngine.Server.Domain.Base;
using SearchEngine.Shared.Enum.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Search
{
    public class SearchableClient : Searchable
    {
        public SearchableClient(Client cliente)
        {
            this.Id = cliente.Id;
            this.Description = $"{cliente.Name} {cliente.SurName}";
            this.Title = Description;
            this.Number = Id;
            this.Type = (int)SearchableTypes.Client;
        }

        public override int Id { get;set; }
        public override string Title { get;set; }
        public override string Description { get;set; }
        public override int Type { get;set; }
        public override int Number { get;set; }
    }
}
