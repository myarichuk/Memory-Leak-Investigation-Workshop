using ShoppingCartService.Model;

namespace ShoppingCartService.Modules
{
    public class OrderModule : RepositoryModule<Order>
    {
        public OrderModule(DataHandler dataHandler) : base(dataHandler)
        {
        }
    }
}
