using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Components.Services;
using Isaac_AnomalyService.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Isaac_AnomalyService.Data;
using Isaac_AnomalyService.Logic;

namespace Isaac_AnomalyService
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
            //services.AddSingleton<MqttConnection>();
            services.AddSingleton<FluxConnection>();
            services.AddTransient<OutlierAlgo>();
            services.AddTransient<AnomalyService>();

            services.AddControllers();
            var connectionString = Configuration["MySQL:ConnectionString"];
            services.AddDbContext<Isaac_AnomalyServiceContext>(options =>
                    options.UseMySql(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
