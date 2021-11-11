using Api.Configurations;
using ApiConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddLogger();

            services.AddDependenciesResolver(_environment);

            services.AddApiConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment() && env.EnvironmentName != "Testing")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }
            else if (env.EnvironmentName != "Testing")
            {
                app.UseExceptionHandler("/error");
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/v1/sample/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseApiConfiguration(env);
        }
    }
}
