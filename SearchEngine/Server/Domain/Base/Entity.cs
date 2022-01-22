using SearchEngine.Server.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Base
{
    public abstract class Entity : IBaseEntity
    {
        public int Id { get;set; }
        public DateTime CreationDate { get;set; }
    }
}
