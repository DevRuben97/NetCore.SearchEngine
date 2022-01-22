using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Shared.DTOs
{
   public class FacturaDto
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int Numero { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }
    }
}
