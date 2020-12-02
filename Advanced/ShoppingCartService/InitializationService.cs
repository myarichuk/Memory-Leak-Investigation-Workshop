using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartService
{
    public class InitializationService : IHostedService
    {
        private readonly DataHandler _dataHandler;

        public InitializationService(DataHandler dataHandler) => _dataHandler = dataHandler;

        public async Task StartAsync(CancellationToken cancellationToken) => 
            await _dataHandler.EnsureSampleDataAsync();

        public Task StopAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
