using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchEngine.Server.Application.Services;
using SearchEngine.Server.Application.Services.Jobs;
using SearchEngine.Server.Application.Services.Search;
using SearchEngine.Server.Domain.Interfaces;
using SearchEngine.Server.Infraestructure.DataAccess;
using SearchEngine.Server.Infraestructure.Repository;
using SearchEngine.Server.Infraestructure.Search;
using System.Linq;

namespace SearchEngine.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(opts =>
            {
                opts.AddPolicy("All", s => s.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });


            services.AddControllersWithViews();
            services.AddRazorPages();

            //Db context:
            services.AddDbContext<AppDbContext>();

            //Add the unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Base Services:

            services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));

            //Add the search manager service:
            services.AddTransient<ISearchManager, SearchManager>();

            //Search Service
            services.AddScoped<ISearchService, SearchService>();

            //Add the index service:
            services.AddHostedService<SearchIndexerHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("All");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
