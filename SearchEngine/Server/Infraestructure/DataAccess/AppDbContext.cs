using Microsoft.EntityFrameworkCore;
using SearchEngine.Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Infraestructure.DataAccess
{
    public class AppDbContext: DbContext
    {
        public DbSet<SearchEngine.Server.Domain.Client> Clients { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=db;Initial Catalog=SearchEngine;Integrated Security=True;Password=Brilinkrt1257");
        }
    }
}
