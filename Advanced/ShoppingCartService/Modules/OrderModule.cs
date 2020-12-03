using ShoppingCartService.Model;

namespace ShoppingCartService.Modules
{
    public class OrderModule : RepositoryModule<Order>
    {
        public OrderModule(DataHandler dataHandler, Microsoft.Extensions.Caching.Memory.IMemoryCache cache) : base(dataHandler, cache)
        {
        }
    }
}
