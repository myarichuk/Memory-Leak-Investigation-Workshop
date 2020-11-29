using Bogus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace MysteryApp5
{
    public class Person
    {
        public string Name { get; set; }

        public string ReportsTo { get; set; }

        public int SomeData { get; set; }
    }

    public class StatisticsHolder
    {
        public double SumOfData { get; private set; }

        public void ProcessData(List<Person> data, Person targetPerson)
        {
            try
            {
                var person = data.Find(d => d.ReportsTo == targetPerson.Name);
                if (person != null)
                    SumOfData += person.SomeData;
            }
            catch (Exception ex)
            {
            }
        }

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

            var peopleFaker = new Faker<Person>()
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.ReportsTo, f => f.Name.FullName())
                .RuleFor(p => p.SomeData, f => f.Random.Number(0, 100));

            var dataHolder = new StatisticsHolder();
            var peopleList = peopleFaker.Generate(5_000_000).ToList();

            var task = Task.Run(async () =>
            {
                while(!mre.IsSet)
                {
                    var person = peopleFaker.Generate();
                    dataHolder.ProcessData(peopleList, person);
                }
            });

            Console.ReadKey();
            mre.Set();

            Task.WaitAll(task, memoryTask);
            Console.WriteLine("OK, bye!");
        }
    }
}
