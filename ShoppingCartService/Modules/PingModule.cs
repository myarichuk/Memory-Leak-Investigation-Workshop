using Carter;
using System.Text;

namespace ShoppingCartService
{
    public class PingModule : CarterModule
    {
        public PingModule()
        {
            Get("/ping", async (req, res) => await res.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("pong!")));
        }
    }
}
