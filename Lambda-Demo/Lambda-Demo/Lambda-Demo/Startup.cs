using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lambda_Demo
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*
            app.Run(async context =>
            {
                for (int i = 0; i < 10; ++i)
                {
                    await Task.Delay(1000);
                    await context.Response.WriteAsync($"{i}{Environment.NewLine}");
                }
            });
            */

            app.Run(async context =>
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";

                if (context.Items.TryGetValue(AbstractAspNetCoreFunction.LAMBDA_CONTEXT, out object lambdaContextObj) &&
                    lambdaContextObj is ILambdaContext lambdaContext)
                {
                    context.Response.Headers["AwsRequestId"] = lambdaContext?.AwsRequestId;
                }

                await using var stream = File.OpenRead("casino-com.html");

                await stream.CopyToAsync(context.Response.Body);
            });
        }
    }
}