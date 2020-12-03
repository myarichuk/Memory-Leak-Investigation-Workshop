using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Raven.Embedded;
using ShoppingCartService.Utils;

namespace ShoppingCartService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCarter();
            services.AddHostedService<ActivitySimulationService>();
            services.AddHostedService<InitializationService>();

            services.RegisterAssemblyTypes<IActivityGenerator>(ServiceLifetime.Transient);
            services.AddSingleton(_ =>
            {
                EmbeddedServer.Instance.StartServer();
                return EmbeddedServer.Instance.GetDocumentStore("ShoppingCart");
            });

            //shared cache between all endpoints
            services.AddSingleton<IMemoryCache, InMemoryCache>();
            services.AddTransient<DataHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/errorhandler");

            app.UseRouting();
            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}
