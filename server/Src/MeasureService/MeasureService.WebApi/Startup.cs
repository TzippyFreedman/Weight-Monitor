using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeasureService.Data;
using MeasureService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeasureService.WebApi
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
            services.AddScoped<IMeasureService, Services.MeasureService>();
            services.AddScoped<IMeasureRepository, MeasureRepository>();

            services.AddDbContext<MeasureDbContext>
              (options => options
              .UseSqlServer(Configuration.GetConnectionString("weightMonitorSubscriberDBConnectionString")));

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    builder =>
                    {
                        builder.WithOrigins( "http://localhost:4200");
                        builder.AllowCredentials();
                        builder.AllowAnyHeader();
                        builder.WithMethods("GET", "POST", "PUT");

                    });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("MyPolicy" );

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
