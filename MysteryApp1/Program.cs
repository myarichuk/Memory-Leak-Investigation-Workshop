using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp1
{
    static class Program
    {
        public class ApplicationObjectA
        {
            public int Number { get; set; }
        }

        public class ApplicationObjectB
        {
            public float Number { get; set; }
        }

        public class ApplicationObjectC
        {
            public float Number { get; set; }
        }

        public class ApplicationObjectD
        {
            public string Foo { get; }

            public ApplicationObjectD()
            {
                Foo = new string('a', 128);
            }
        }

        static void Main(string[] args)
        {

            var refs = new List<object>();
            var mre = new ManualResetEventSlim();
            var i = 0;

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
                while (!mre.IsSet)
                {
                    if (i % 5 == 0)
                        new ApplicationObjectB();
                    if (i % 8 == 0)
                        new ApplicationObjectA();

                    if (i % 10 == 0)
                        new ApplicationObjectC();

                    if (i % 50 == 0)
                        refs.Add(new ApplicationObjectD());

                    i++;
                }

            });

            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
        }
    }
}
