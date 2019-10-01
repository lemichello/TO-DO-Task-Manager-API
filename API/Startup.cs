using API.Helpers;
using AutoMapper;
using DAL;
using DAL.Repositories.Abstraction;
using DAL.Repositories.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EfContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<Profile, MapperProfile>();

            services.AddAuthentication();
            services.AddDataProtection()
                .SetApplicationName("TO-DO Task Manager API");
            services.AddHttpsRedirection(options => options.HttpsPort = 8999);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}