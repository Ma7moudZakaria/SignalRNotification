using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Hubs;
using WebApplication4.Models;

namespace WebApplication4
{
    public class Startup
    {
        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true));
            });
            services.AddControllers();
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(60);
            });
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {

            //                          builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((x) => true);
            //                      });
            //});
            services.AddTransient<NotificationHub>();
            //services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowAnyOrigin()
            //        .AllowCredentials();
            //}));

            //services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .WithOrigins("http://localhost:12345")
            //        .DisallowCredentials();
            //}));
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
            //app.UseCors() ;
            app.UseCors("CorsPolicy");
            //app.UseCors(builder =>
            //{
            //    builder.WithOrigins("http://localhost:4200")
            //    .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            //});

            app.UseAuthorization();

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<NotificationHub>("/integrationhub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/NotificationHub/negotiate");
            });
        }
    }
}
