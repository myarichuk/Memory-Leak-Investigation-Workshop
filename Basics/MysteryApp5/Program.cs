using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp5
{
    public class TimerEventDispatcher
    {
        private System.Timers.Timer _timer;

        public TimerEventDispatcher(System.Timers.Timer timer)
        {

            _timer = timer;
            _timer.Elapsed += (_, e) => OnAlarmNow(e);
        }

        public event EventHandler TimerElapsedNow;


        protected virtual void OnAlarmNow(EventArgs e) => 
            TimerElapsedNow?.Invoke(this, e);
    }

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

            var task = Task.Run(() =>
            {
                var timer = new System.Timers.Timer(500);
                timer.Start();

                while(!mre.IsSet)
                {
                    var eventDispatcher = new TimerEventDispatcher(timer);
                    eventDispatcher.TimerElapsedNow += EventDispatcher_TimerElapsedNow;

                    Thread.Sleep(50);
                }
            });

            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
            Console.WriteLine("OK, bye!");
        }

        private static void EventDispatcher_TimerElapsedNow(object sender, EventArgs e)
        {
        }
    }
}
