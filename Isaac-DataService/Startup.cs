using Isaac_DataService.Components.Connections;
using Isaac_DataService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Isaac_DataService
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
            services.AddSingleton<MqttConnection>();
            services.AddSingleton<IFluxConnection, FluxConnection>();
            services.AddSingleton<InfluxService>();
            services.AddSingleton<DataService>();
            services.AddSingleton<DataOutputService>();
            services.AddSingleton<IHostedService, DataOutputService>(
                serviceProvider => serviceProvider.GetService<DataOutputService>());
            services.AddSingleton<IHostedService, DataService>(
                serviceProvider => serviceProvider.GetService<DataService>());

            
            services.AddLogging();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}