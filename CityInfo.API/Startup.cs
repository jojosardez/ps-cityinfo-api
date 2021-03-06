﻿using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {

        #region .Net Core 1 Configuration File Accessing

        /* 
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            // Add config file
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariable();
            Configuration = builder.Build();
        }
        */

        #endregion

        #region .Net Core 2 Configuration File Accessing

        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC middleware
            services.AddMvc()

                // Formmater option to support Accept headers in request that asks for XML
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));

            // Serialization option to return JSON property names in their original casing.
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var casterResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        casterResolver.NamingStrategy = null;
            //    }
            //});

            // Transient - new instance will be created everytime they are accessed
            // Scoped - new instance per request - lifetime is within the request
            // Singleton - created the first time it is called. Only one instance is created while the service is running

            // Register mail service in the built-in dependency injection container
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            // Register database context in the built-in dependency injection container
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"]
                .Replace(@"\\", @"\");
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));

            // Register repository in the built-in dependency injection container
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CityInfoContext cityInfoContext)
        {
            // Add logger providers to the request pipeline
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            loggerFactory.AddNLog();

            // Add exception page to the request pipeline
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            // Seed the database
            cityInfoContext.EnsureSeedDataForContext();

            // Add status code pages to the request pipeline
            app.UseStatusCodePages();

            // Configure mappings
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                cfg.CreateMap<Entities.City, Models.CityDto>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
                cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            });

            // Add MVC middleware to the request pipeline
            app.UseMvc();


            // Default implementation
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
