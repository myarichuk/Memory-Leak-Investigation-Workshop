using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShoppingCartService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAssemblyTypes<T>(this IServiceCollection services, ServiceLifetime lifetime, List<Func<TypeInfo, bool>> predicates = null)
        {
            var scanAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            scanAssemblies.SelectMany(x => x.GetReferencedAssemblies())
                .Where(t => !scanAssemblies.Any(a => a.FullName == t.FullName))
                .Distinct()
                .ToList()
                .ForEach(x => scanAssemblies.Add(AppDomain.CurrentDomain.Load(x)));

            var interfaces = scanAssemblies
                .SelectMany(o => o.DefinedTypes
                    .Where(x => x.IsInterface && x != typeof(T) && typeof(T).IsAssignableFrom(x))
                );

            foreach (var @interface in interfaces)
            {
                var types = scanAssemblies
                    .SelectMany(o => o.DefinedTypes
                        .Where(x => x.IsClass && @interface.IsAssignableFrom(x))
                    );

                if (predicates?.Count() > 0)
                {
                    foreach (var predict in predicates)
                    {
                        types = types.Where(predict);
                    }
                }

                foreach (var type in types)
                {
                    services.Add(new ServiceDescriptor(
                        @interface,
                        type,
                        lifetime));
                }
            }

            return services;
        }
    }
}
