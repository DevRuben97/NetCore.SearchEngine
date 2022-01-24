using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration configuration;

        public AppDbContext(IConfiguration config)
        {
            this.configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("sqlServer"));
        }
    }
}
