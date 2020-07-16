using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.OpenApi.Models;

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

            services.AddAutoMapper(typeof(ApiMappingProfile), typeof(DataMappingProfile));
            //services.AddAutoMapper(typeof(Startup));



            var swaggerTitle = Configuration["AppSettings:Swagger:Title"];
            var swaggerName = Configuration["AppSettings:Swagger:Name"];

            services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc(swaggerName, new OpenApiInfo()
                        {
                            Title = swaggerTitle,
                            Description = Configuration["AppSettings:Swagger:OpenApiContact:Description"],
                            Contact = new OpenApiContact()
                            {
                                Name = Configuration["AppSettings:Swagger:OpenApiContact:Name"],
                                Email = Configuration["AppSettings:Swagger:OpenApiContact:Email"]

                            }
                        });
                    });


            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                        builder.AllowCredentials();
                        builder.AllowAnyHeader();
                        builder.WithMethods("GET", "POST", "PUT");

                    });

            });
            //var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));

            /* services.AddAuthentication(x =>
             {
                 x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = false,
                     ValidateAudience = false,

                     ClockSkew = TimeSpan.Zero
                 };
             });
 */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var swaggerName = Configuration["AppSettings:Swagger:SwaggerName"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("MyPolicy");
                app.UseHsts();
            }

            // app.UseErrorHandlingMiddleware();

            //not sure what it does
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{swaggerName}/swagger.json", swaggerName);
            });


            app.UseRouting();

            /*app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies[".AspNetCore.Application.Id"];
                if (!string.IsNullOrEmpty(token))
                {
                    //context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    //context.Response.Headers.Add("X-Xss-Protection", "1");
                    //context.Response.Headers.Add("X-Frame-Options", "DENY");
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                    //   seDefaultCredentials = true

                }
                await next();
            });*/

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
