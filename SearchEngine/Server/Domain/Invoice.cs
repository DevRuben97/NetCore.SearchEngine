using SearchEngine.Server.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain
{
    public class Invoice: Entity
    {
        public int ClientId { get; set; }

        public int Number { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public Client Client { get; set; }
    }
}
