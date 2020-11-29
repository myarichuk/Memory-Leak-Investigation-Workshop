using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp3
{
    //TODO: try implement what is described here https://github.com/dotnet/runtime/issues/25197#issuecomment-371505770
    public static class Program
    {
        static void Main(string[] args)
        {
            var mre = new ManualResetEventSlim();
            var memoryTask = Task.Run(() =>
            {
                while (!mre.IsSet)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"Process ID: {Environment.ProcessId}");
                    Console.Write("Working, press any key to stop...");

                    Console.WriteLine();
                    GCUtils.PrintInfoAboutLastCycle();

                    Thread.Sleep(1000);
                }
            });

            var task = Task.Run(async () =>
            {
                //this cts is simulation of a service-wide one that will fire when the service needs to shut down
                var systemCts = new CancellationTokenSource();

                var mockHttp = new MockHttpMessageHandler();

                mockHttp.When("/")
                        .Respond(HttpStatusCode.OK); 

                using (var httpClient = mockHttp.ToHttpClient()) //simulate "static" http client
                {
                    while (!mre.IsSet)
                    {
                        //simulate API linking cancellation token source with main one that indicates system lifetime
                        var localCts = CancellationTokenSource.CreateLinkedTokenSource(systemCts.Token);
                        localCts.CancelAfter(2000); 

                        using (var content = new StringContent(JsonSerializer.Serialize(new { Foo = "Bar" })))
                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri("http://foobar.non-existing", UriKind.RelativeOrAbsolute)))
                        {
                            request.Content = content;
                            using (var result = await httpClient.SendAsync(request, localCts.Token))
                            {
                                //do stuff with the result
                            }
                        }
                    }
                }
            });

            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
            Console.WriteLine("OK, bye!");
        }
    }
}
