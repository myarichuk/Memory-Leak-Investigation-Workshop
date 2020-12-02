using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Raven.Embedded;
using ShoppingCartService.Model;

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

            services.AddSingleton<DataHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}
