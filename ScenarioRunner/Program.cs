using System;

namespace ScenarioRunner
{
    static class Program
    {
        enum State
        {
            Idle,
            Running
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Issue Scenario Runner For Memory Leak Investigation Workshop");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine($"Process ID: {Environment.ProcessId} - useful for attaching debuggers :)");
            Console.WriteLine();


        }
    }
}
