using Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory =
                    (context =>
                    {
                        return new BadRequestObjectResult(new
                        {
                            success = false,
                            errors = context.ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage))
                        });
                    });
            });
            services.AddSwagger();

            services.AddLogger();

            services.AddDependenciesResolver(_environment);

            services.AddApiConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName != "Testing")
            {
                var baseUrl = "";
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();
                else
                {
                    app.UseExceptionHandler("/error");
                    baseUrl = "/v1/sample";
                }
                app.UseSwaggerConfig(baseUrl);
                app.UseSwaggerUI(c => c.SwaggerEndpoint(baseUrl + "/swagger/v1/swagger.json", "API v1"));
            }

            app.UseApiConfiguration();
        }
    }
}
