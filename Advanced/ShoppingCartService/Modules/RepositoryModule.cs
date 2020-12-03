using Carter;
using Carter.Request;
using Carter.Response;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCartService.Modules
{
    public abstract class RepositoryModule<TDocument> : CarterModule
    {
        internal JsonSerializerOptions Options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public string CollectionName => typeof(TDocument).Name;

        public RepositoryModule(DataHandler dataHandler, IMemoryCache cache)
        {
            Get($"/{CollectionName}", async (req, res) =>
            {
                var id = req.Query.As<string>("id", null) ?? throw new ArgumentException($"Empty or query parameter", "id");
                if (cache.TryGetValue(id, out object doc))
                {
                    await res.Negotiate(doc);
                    return;
                }

                doc = dataHandler.GetById<TDocument>(id);

                if (doc != null)
                {
                    cache.Set(id, doc);
                    await res.Negotiate(doc);
                }
            });

            Post($"/{CollectionName}", async (req, res) =>
            {
                string json = await req.Body.AsStringAsync();
                var id = req.Query.As<string>("id");
                                
                var document = JsonSerializer.Deserialize<TDocument>(json, Options);

                if (id == null)
                    dataHandler.Put(document);
                else
                    dataHandler.Put(id, document);
            });

            Get($"/{CollectionName}/list", async (req, res) =>
            {
                var skip = int.Parse(req.Query["skip"].FirstOrDefault() ?? "0");
                var take = int.Parse(req.Query["take"].FirstOrDefault() ?? "1024");
                var docList = dataHandler.Query<TDocument>(skip, take).ToList();
                await res.Negotiate(docList);
            });

        }
    }
}
