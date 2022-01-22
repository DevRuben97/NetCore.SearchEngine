using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Domain.Interfaces
{
  public  interface ISearchable
    {
         IEnumerable<Field> GetFields();
    }
}
