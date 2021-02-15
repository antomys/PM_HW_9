using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            services.AddSingleton<ISettings, Settings>();
            services.AddTransient<IPrimeAlgorithm, PrimeAlgorithm>();
        }
        
       
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
                    await context.Response.WriteAsync(" PM_HW_9, Web service <<Prime Numbers>>\n Volokhovych Ihor ");
                });
                endpoints.MapGet("/primes/{number:int}", async context =>
                {
                    var service 
                        = context.RequestServices.GetRequiredService<IPrimeAlgorithm>();
                   
                    var item = (string) context.Request.RouteValues["number"];
                    int.TryParse(item, out var number);
                    
                    var isPrime = await service.IsPrime(number);

                    context.Response.StatusCode = isPrime
                        ? (int) HttpStatusCode.OK
                        : (int) HttpStatusCode.NotFound;
                });
                endpoints.MapGet("/primes", async context =>
                {
                    var service 
                        = context.RequestServices.GetRequiredService<IPrimeAlgorithm>();
                    
                    var settings 
                        = context.RequestServices.GetRequiredService<ISettings>();
                    
                    var inputPrimeFrom = context.Request.Query["from"].FirstOrDefault();
                    var inputPrimeTo = context.Request.Query["to"].FirstOrDefault();
                    
                    if(inputPrimeFrom == null || inputPrimeTo == null)
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    

                    if (int.TryParse(inputPrimeFrom, out var primeFrom) &&
                        int.TryParse(inputPrimeTo, out var primeTo))
                    {
                        settings.PrimeFrom = primeFrom;
                        settings.PrimeTo = primeTo; 
                        
                        /*if(await service.GetPrimes())
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }*/
                        context.Response.StatusCode = await service.GetPrimes()
                            ? context.Response.StatusCode = (int)HttpStatusCode.OK
                            : context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                });
            });
        }
    }
}