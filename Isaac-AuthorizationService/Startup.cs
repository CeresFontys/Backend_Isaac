using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Interfaces;
using Isaac_AuthorizationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace Isaac_AuthorizationService
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
            services.AddCors();
            //services.AddControllers();
            services.AddControllers();

            var connectionString = Configuration["MySQL:ConnectionString"];
          
            services.AddDbContext<ApplicationDbContext>(o => o.UseMySql(connectionString));
            services.AddScoped<IUserService, AuthService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseMySql(Configuration.GetConnectionString("MySQL:ConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<IpSafeListMiddleware>(Configuration["IpSafeList"]);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}