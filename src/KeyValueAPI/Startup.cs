using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyValueAPI.Models;
using KeyValueAPI.DAL;
using KeyValueAPI.Repositories.Interface;
using KeyValueAPI.Repositories;
using KeyValueAPI.ActionFilters;
using KeyValueAPI.Extensions;

namespace KeyValueAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DictionaryContext>(opts => opts.UseInMemoryDatabase("TestDatabase"));
            services.AddScoped<IRepository<DictionaryItem>, DictionaryRepository>();
            services.AddControllers();
            services.AddScoped<ValidationFilter>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KeyValueAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeyValueAPI v1"));
            }

            app.ConfigureExceptionHandler(logger);
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
