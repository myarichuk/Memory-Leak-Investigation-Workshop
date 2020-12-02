using ShoppingCartService.Model;

namespace ShoppingCartService.Modules
{
    public class CategoryModule : RepositoryModule<Category>
    {
        public CategoryModule(DataHandler dataHandler) : base(dataHandler)
        {
        }
    }
}
