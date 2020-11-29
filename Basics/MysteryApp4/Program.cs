using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp4
{
    class Program
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
                
                while(!mre.IsSet)
                {
                    using var cts = new CancellationTokenSource();
                    Func<Task> fun = async () => await Task.Delay(100, cts.Token);

                    await fun();

                    // uncomment the following lines to see interesting allocation pattern changes!
                    //GC.Collect(2);
                    //GC.WaitForPendingFinalizers();
                    //GC.Collect();
                }
            });

            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
            Console.WriteLine("OK, bye!");
        }
    }
}
