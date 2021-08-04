using FootballCrimeStatistics.Logic;
using FootballCrimeStatistics.Logic.Crime;
using FootballCrimeStatistics.Logic.Football;
using FootballCrimeStatistics.Logic.PostcodeLookup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCrimeStatistics
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
            services.AddDistributedMemoryCache();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FootballCrimeStatistics", Version = "v1" });
            });

            services.AddHttpClient<FootballClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("Services:FootballData:Endpoint"));
                c.DefaultRequestHeaders.Add("X-Auth-Token", Configuration.GetValue<string>("Services:FootballData:ApiToken"));
            });

            services.AddHttpClient<PostcodeClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("Services:PostcodeData:Endpoint"));
            });

            services.AddHttpClient<CrimeClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("Services:CrimeData:Endpoint"));
            });

            services.AddTransient<IFootballService, FootballService>();
            services.AddTransient<IPostcodeLookupService, PostcodeLookupService>();
            services.AddTransient<ICrimeService, CrimeService>();
            services.AddTransient<IFootballCrimeService, FootballCrimeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FootballCrimeStatistics v1"));


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
