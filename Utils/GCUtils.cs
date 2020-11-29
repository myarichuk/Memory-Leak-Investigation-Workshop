using System;
using System.Diagnostics;

namespace Utils
{
    public static class GCUtils
    {
        public static void PrintInfoAboutLastCycle()
        {
            var memInfo = GC.GetGCMemoryInfo();
            Console.WriteLine("Information about last GC cycle");
            Console.WriteLine("===============================");

            if (memInfo.PauseDurations.Length == 1 || (memInfo.PauseDurations.Length == 2 && memInfo.PauseDurations[1].TotalMilliseconds == 0))
                Console.WriteLine($"GC Pause Duration: {memInfo.PauseDurations[0].TotalMilliseconds}ms  ");
            else if (memInfo.PauseDurations.Length == 2)
                Console.WriteLine($"GC Pause Duration: {memInfo.PauseDurations[0].TotalMilliseconds}ms, {memInfo.PauseDurations[1].TotalMilliseconds}ms   ");

            Console.WriteLine($"GC cycle count (Gen 0): {GC.CollectionCount(0)}");
            Console.WriteLine($"GC cycle count (Gen 1): {GC.CollectionCount(1)}");
            Console.WriteLine($"GC cycle count (Gen 2): {GC.CollectionCount(2)}");

            Console.WriteLine($"Time spent in GC: {memInfo.PauseTimePercentage}%   ");
            Console.WriteLine($"{nameof(memInfo.Concurrent)}: Was the GC cycle concurrent? (BGC) {memInfo.Concurrent}  ");
            Console.WriteLine($"{nameof(memInfo.Compacted)}: Was the GC cycle compacting? {memInfo.Compacted}  ");
            Console.WriteLine();
            Console.WriteLine($"Physical memory usage: {Environment.WorkingSet / 1024} kb");
            
        }
    }
}
