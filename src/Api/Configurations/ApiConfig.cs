using Api.Converters;
using Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.Jwt.AspNetCore;
using System.Collections.Generic;
using System.Globalization;

namespace Api.Configurations
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

            services.AddLogging();

            services.AddJwtConfiguration();

            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
                ));

            return services;
        }

        private static RequestLocalizationOptions GetLocalizationOptions()
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("pt-BR"),
            };
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                ApplyCurrentCultureToResponseHeaders = true,
            };
            return options;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseMigrations();

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();

            app.UseJwksDiscovery();

            app.UseHealthChecks("/health");

            app.UseCors("CorsPolicy");

            app.UseRequestLocalization(GetLocalizationOptions());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
