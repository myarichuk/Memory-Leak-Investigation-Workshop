using System;
using System.Collections.Generic;

namespace ShoppingCartService.Model
{

    public class ShoppingCart
    {
        public string Id { get; set; }

        public bool IsPaid => Invoice != null;

        public Invoice Invoice { get; set; }

        public List<Product> Products { get; set; }
    }
}
