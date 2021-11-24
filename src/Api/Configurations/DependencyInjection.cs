using Business.Interfaces.Repositories;
using Core.Interfaces;
using Core.Notificacoes;
using Core.Secrets;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Api.Configurations
{
    public static class DependencyInjection
    {
        public static void AddDependenciesResolver(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddScoped<SampleContext>();
            services.AddScoped<INotificador, Notificador>();

            services.AddAutoMapper(new Assembly[] {
                Assembly.Load("Api"),
                Assembly.Load("Business")
            });

            RegisterSecrets(services);

            RegisterRepository(services);

            RegisterService(services);
        }

        public static void UseMigrations(this IApplicationBuilder app)
        {
            var contexts = GetContextRepositoryClasses();
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            foreach (var @class in contexts)
            {
                var context = (DbContext)serviceScope.ServiceProvider.GetService(@class);
                context.Database.Migrate();
            }
        }

        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            services.AddSingleton(typeof(ILogger), logger);

            return services;
        }

        private static void RegisterSecrets(IServiceCollection services)
        {
            var secrets = GetSecretsClasses();
            Inject(secrets, (t, i) => services.AddScoped(i, t));
        }

        private static void RegisterService(IServiceCollection services)
        {
            var serviceClasses = GetServiceClasses();
            Inject(serviceClasses, (t, i) => services.AddScoped(i, t));
        }

        private static void RegisterRepository(IServiceCollection services)
        {
            var repositories = GetRepositoryClasses();
            Inject(repositories, (t, i) => services.AddTransient(i, t));
        }

        private static void Inject(IEnumerable<Type> types, Func<Type, Type, IServiceCollection> injectFunc)
        {
            foreach (var @class in types)
            {
                var @interfaces = @class.GetInterfaces();

                foreach (var @interface in @interfaces)
                    if (@interface != null)
                        injectFunc(@class, @interface);
            }
        }

        private static IEnumerable<Type> GetSecretsClasses() => GetClasses("Core", "Core.Secrets")
            .Where(type => type.IsAssignableTo(typeof(Secrets)));

        private static IEnumerable<Type> GetServiceClasses() => GetClasses("Service", "Service")
            .Where(type => type.IsAssignableTo(typeof(BaseService)));

        private static IEnumerable<Type> GetRepositoryClasses() => GetClasses("Infrastructure", "Infrastructure.Repositories")
            .Where(type => type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IRepository<>)));

        private static IEnumerable<Type> GetContextRepositoryClasses() => GetClasses("Infrastructure", "Infrastructure.Contexts")
            .Where(type => type.IsAssignableTo(typeof(DbContext)));

        private static IEnumerable<Type> GetClasses(string nomeCamada, string @namespace) => Assembly.Load(nomeCamada).GetTypes()
            .Where(type => type.IsClass
            && !type.IsAbstract
            && type.Namespace == @namespace
            && type.GetCustomAttribute<CompilerGeneratedAttribute>() == null);
    }
}
