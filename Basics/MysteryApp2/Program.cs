using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp2
{
    public class AMysteryClass
    {
        private readonly int _delay;

        public AMysteryClass(int delay) => _delay = delay;
        ~AMysteryClass() => Thread.Sleep(_delay);
    }
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

            var task = Task.Run(() =>
            {
                int x = 0;
                var random = new Random();
                while (!mre.IsSet)
                {
                    Task.Run(() =>
                    {
                        var _ = new AMysteryClass(random.Next(100, 5000));
                    });
                }
            });

            //166x86
            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
            Console.WriteLine("OK, bye!");
        }
    }
}
