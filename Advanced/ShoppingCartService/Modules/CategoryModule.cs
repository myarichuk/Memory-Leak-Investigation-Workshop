using ShoppingCartService.Model;

namespace ShoppingCartService.Modules
{
    public class CategoryModule : RepositoryModule<Category>
    {
        public CategoryModule(DataHandler dataHandler, Microsoft.Extensions.Caching.Memory.IMemoryCache cache) : base(dataHandler, cache)
        {
        }
    }
}
