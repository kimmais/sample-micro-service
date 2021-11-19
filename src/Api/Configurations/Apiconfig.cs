using Api.Configurations;
using Api.Converters;
using Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NetDevPack.Security.Jwt.AspNetCore;
using System.Collections.Generic;

namespace ApiConfiguration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                o.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
                opts.JsonSerializerOptions.Converters.Add(new CustomIntConverter());
                opts.JsonSerializerOptions.Converters.Add(new CustomLongConverter());
                opts.JsonSerializerOptions.Converters.Add(new CustomShortConverter());
                opts.JsonSerializerOptions.Converters.Add(new CustomDecimalConverter());
                opts.JsonSerializerOptions.Converters.Add(new CustomGuidConverter());
            });

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });
            });

            services.AddJwtConfiguration();

            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
                ));

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMigrations();

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();

            app.UseJwksDiscovery();

            app.UseHealthChecks("/health");

            app.UseCors("CorsPolicy");

            if (env.EnvironmentName != "Testing")
                app.UseSwagger();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, string baseRouter)
        {
            return app.UseSwagger(options =>
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = baseRouter, Description = "Default" } }));
        }
    }
}