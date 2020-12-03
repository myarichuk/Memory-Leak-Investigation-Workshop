using Carter;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using ShoppingCartService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartService.Modules
{
    //public API
    public class ShoppingCartModule : CarterModule
    {
        private static int CheckoutRequestCounter = 0;
        private readonly static RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager(1024, 2, int.MaxValue, true);

        public ShoppingCartModule(DataHandler dataHandler, IMemoryCache cache, ILogger<ShoppingCartModule> log) : base("/cart")
        {
            Get("/", (req, res) =>
            {
                if(TryGetShoppingCartById(req, dataHandler, out var cart))
                    return res.Negotiate(cart);

                res.StatusCode = 404;
                return res.AsJson(new
                {
                    Message = "Shopping Cart with the specified cartId was not found"
                });
            });

            Post("/", (req, res) =>
            {
                var cart = new ShoppingCart
                {
                    Products = new List<Product>()
                };
                dataHandler.Put(cart);

                res.StatusCode = 201; //created
                return res.AsJson(new { cart.Id });
            });

            Put("/", (req, res) =>
            {
                if(!TryGetShoppingCartById(req, dataHandler, out var cart))
                {
                    res.StatusCode = 404;
                    return res.AsJson(new
                    {
                        Message = "Shopping Cart with the specified cartId was not found"
                    });
                }

                var productIds = req.Query.AsMultiple<string>("productId").ToList();

                var products = GetProductsByIds(productIds, dataHandler, cache);
                cart.Products.AddRange(products);

                dataHandler.Put(cart);

                return Task.CompletedTask;
            });

            Post("/checkout", (req, res) =>
            {
                Interlocked.Increment(ref CheckoutRequestCounter);

                _streamManager.StreamDisposed += () => log.LogDebug("Recyclable Stream Disposed");
                _streamManager.StreamFinalized += () => log.LogDebug("Recyclable Stream Finalized");

                if (!TryGetShoppingCartById(req, dataHandler, out var cart))
                {
                    res.StatusCode = 404;
                    return res.AsJson(new
                    {
                        Message = "Shopping Cart with the specified cartId was not found"
                    });
                }

                var paymentId = req.Query.As("paymentId", Guid.Empty);
                if(paymentId == Guid.Empty)
                    throw new InvalidOperationException("Missing or invalid payment Id, cannot continue with checkout. Check the 'paymentId' parameter and make sure to include proper ID of the payment (a Guid)");

                //assume we also validate the payment Id
                var invoice = new Invoice
                {
                    PaymentNumber = paymentId
                };

                cart.Invoice = invoice;
                dataHandler.Put(cart.Id, cart);

                
                //var order = new Order
                //{
                //    OrderedAt = DateTime.UtcNow,
                //    ShipTo = new Address
                //};

                return Task.CompletedTask;
            });

            Delete("/", (req, res) =>
            {
                if (!TryGetShoppingCartById(req, dataHandler, out var cart))
                {
                    res.StatusCode = 404;
                    return res.AsJson(new
                    {
                        Message = "Shopping Cart with the specified cartId was not found"
                    });
                }

                dataHandler.DeleteById(cart.Id);

                res.StatusCode = 202;
                return Task.CompletedTask;
            });
        }


        private bool TryGetShoppingCartById(HttpRequest req, DataHandler dataHandler, out ShoppingCart cart)
        {
            var cartId = req.Query.As<string>("cartId", null) ?? throw new ArgumentNullException("cartId");

            cart = dataHandler.GetById<ShoppingCart>(cartId);
            
            return cart != null;
        }

        private IEnumerable<Product> GetProductsByIds(IEnumerable<string> ids, DataHandler dataHandler, IMemoryCache cache)
        {
            foreach(var id in ids)
            {
                if (cache.TryGetValue(id, out Product product))
                    yield return product;
                else
                {
                    var fetchedProduct = dataHandler.GetById<Product>(id);
                    if (fetchedProduct != null)
                        yield return fetchedProduct;
                }
            }
        }
    }
}
