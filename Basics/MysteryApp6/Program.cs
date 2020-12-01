using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp5
{
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
                while (!mre.IsSet)
                {
                    //note: PhysicalFileProvider under the hood uses unmanaged resources (see https://github.com/dotnet/runtime/blob/master/src/libraries/System.IO.FileSystem.Watcher/src/System/IO/FileSystemWatcher.Win32.cs#L32)
                    try
                    {
                        var fp = new PhysicalFileProvider(Path.GetTempPath());
                        fp.Watch("*.*");
                    }
                    catch (Exception) { }
                }
            });

            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
            Console.WriteLine("OK, bye!");
        }
    }
}