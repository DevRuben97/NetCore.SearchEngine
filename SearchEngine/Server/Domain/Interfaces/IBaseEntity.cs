using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Interfaces
{
    public interface IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
