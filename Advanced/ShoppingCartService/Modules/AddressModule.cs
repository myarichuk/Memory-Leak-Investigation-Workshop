using ShoppingCartService.Model;

namespace ShoppingCartService.Modules
{
    public class AddressModule : RepositoryModule<Address>
    {
        public AddressModule(DataHandler dataHandler) : base(dataHandler)
        {
        }
    }
}
