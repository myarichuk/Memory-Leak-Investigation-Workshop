using Carter;
using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Text;

namespace ShoppingCartService.Modules
{
    public class ErrorModule : CarterModule
    {
        public ErrorModule()
        {
            Get("/error", (req, res) => throw new Exception("oopsie"));

            Get("/errorhandler", async (req, res) =>
            {
                string error = string.Empty;
                var feature = req.HttpContext.Features.Get<IExceptionHandlerFeature>();
                if (feature != null)
                {
                    if (feature.Error is ArgumentNullException)
                    {
                        res.StatusCode = 402;
                    }
                    error = feature.Error.ToString();
                }

                await res.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes($"There has been an unexpected error...{Environment.NewLine}{error}").AsMemory());
            });
        }
    }
}
