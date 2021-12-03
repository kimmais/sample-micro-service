using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Configurations
{
    public static class SwaggerConfig
    {
        private static readonly List<string> _actionsOrder = new() { "GET", "POST", "PUT", "PATCH", "DELETE" };

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1",
                    Contact = new OpenApiContact() { Name = "KimMais", Email = "contato@kimmais.com.br" }
                });

                c.OrderActionsBy((apiDesc) => OrderActionsBy(apiDesc));

                c.AddSecurity();

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                xmlFiles.ToList().ForEach(f => c.IncludeXmlComments(f));
            });

            return services;
        }
        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, string baseRouter)
        {
            return app.UseSwagger(options =>
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = baseRouter, Description = "Default" } }));
        }

        private static string OrderActionsBy(ApiDescription apiDesc)
        {
            var method = apiDesc.HttpMethod.ToUpperInvariant();
            var i = _actionsOrder.IndexOf(method);
            if (i < 0) { i = _actionsOrder.Count; }
            var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
            return $"{controller}_{ i }_{ apiDesc.RelativePath }";
        }

        private static void AddSecurity(this SwaggerGenOptions c)
        {
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
                }
            );
        }
    }
}