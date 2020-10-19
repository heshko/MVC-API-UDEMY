using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository.IRepository;
using AutoMapper;
using System.Reflection;
using System.IO;

namespace ParkyAPI
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
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailsRepository, TrailsRepository>();
       
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("ParkConnection")));
         
            services.AddAutoMapper(typeof(Mappings));
            //services.AddApiVersioning(options =>
            //{
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.DefaultApiVersion = new ApiVersion(1,0);
            //    options.ReportApiVersions = true;
            //});
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("HeshkoSwaggerParks", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Heshko Api swagger",
                    Version = "1"
                });
                var xmlCommandFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommandFile);
                options.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("HeshkoSwaggerTrails", new Microsoft.OpenApi.Models.OpenApiInfo() { 
                
                Title="Parky",
                Version="1",
                Description = "HISHAM PARKY API UDEMY",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                {
                    Email="Shooter.metall@gmail.com",
                    Name ="Hisham Abulil",
                    Url = new Uri("https://www.facebook.com/heshkooo")
                },
                License = new Microsoft.OpenApi.Models.OpenApiLicense()
                {
                    Name ="Api Swagger"
                }
                });

                //options.SwaggerDoc("HeshkoSwaggerTrails", new Microsoft.OpenApi.Models.OpenApiInfo()
                //{

                //    Title = "Trail",
                //    Version = "1",
                //    Description = "HISHAM Trail API UDEMY",
                //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                //    {
                //        Email = "Shooter.metall@gmail.com",
                //        Name = "Hisham Abulil",
                //        Url = new Uri("https://www.facebook.com/heshkooo")
                //    },
                //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                //    {
                //        Name = "Api Swagger"
                //    }
                //});


                var commentXmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentXmlFile = Path.Combine(AppContext.BaseDirectory, commentXmlfile);
                options.IncludeXmlComments(commentXmlFile);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/HeshkoSwaggerParks/swagger.json", "Hisham Api park ");
                options.RoutePrefix = "";
                options.SwaggerEndpoint("swagger/HeshkoSwaggerTrails/swagger.json", "Hisham Api Trails");

            });

          
            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/HeshkoSwagger/swagger.json", "Hisham Api");
            //    options.RoutePrefix = "";
              
            //});
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
