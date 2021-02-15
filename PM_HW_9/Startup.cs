using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PM_HW_9.Services;
using PM_HW_9.Services.Interfaces;
using Serilog;
using Serilog.Events;

namespace PM_HW_9
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISettings, Settings>();
            services.AddTransient<IPrimeAlgorithm, PrimeAlgorithm>();
        }

        /*public void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddSerilog(new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("app.log")
                .CreateLogger());
            
        }*/
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("PM_HW_9, Web service <<Prime Numbers>>\n Volokhovych Ihor ");
                });
            });
        }
    }
}