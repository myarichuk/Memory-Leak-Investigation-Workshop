using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp4
{
    public class CloseableThreadLocal
    {
        [ThreadStatic] 
        private static Dictionary<object, object> slots;

        public static Dictionary<object, object> Slots => 
            slots ??= new Dictionary<object, object>();

        protected virtual object InitialValue() => null;

        public virtual object Get()
        {
            object val;

            if (Slots.TryGetValue(this, out val))
                return val;

            val = InitialValue();
            Set(val);

            return val;
        }

        public virtual void Set(object val) => 
            Slots[this] = val;

        public virtual void Close()
        {
            if (slots != null)// intentionally using the field here, to avoid creating the instance
                slots.Remove(this);
        }
    }

    static class Program
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
            
            ThreadPool.SetMinThreads(1000, 100);

            var task = Task.Run(() =>
            {
                var tl = new CloseableThreadLocal();
                int x = 0;
                while (!mre.IsSet)
                {
                    Task.Run(() =>
                    {
                        tl.Set("hello!");
                        _ = tl.Get();
                        Thread.Sleep(10);
                    });
                    
                    if(++x % 1000 == 0)
                    {
                        GC.Collect(2);
                        GC.WaitForPendingFinalizers();
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
