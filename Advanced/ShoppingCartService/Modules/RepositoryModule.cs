using Carter;
using Carter.Request;
using Carter.Response;
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

        public RepositoryModule(DataHandler dataHandler)
        {
            Get($"/{CollectionName}/{{id}}", async (req, res) =>
            {
                var doc = dataHandler.GetById<TDocument>(req.RouteValues.As<string>("id"));
                await res.Negotiate(doc);
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

            Get($"/{CollectionName}", async (req, res) =>
            {
                var skip = int.Parse(req.Query["skip"].FirstOrDefault() ?? "0");
                var take = int.Parse(req.Query["take"].FirstOrDefault() ?? "1024");
                var docList = dataHandler.Query<TDocument>(skip, take).ToList();
                await res.Negotiate(docList);
            });

        }
    }
}
