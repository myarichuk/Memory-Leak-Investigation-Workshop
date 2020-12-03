using Microsoft.Diagnostics.Runtime;
using System;
using System.IO;
using System.Linq;

namespace ClrmdPlayground
{
    public static class Program
    {
        private static DataTarget _target;
        private static ClrRuntime _runtime;

        static void Main(string[] args)
        {
            var dumpFullPath = "d:\\MysteryApp3.dmp";
            if (File.Exists(dumpFullPath) == false)
                throw new FileNotFoundException($"Dump not found", dumpFullPath);

            _target = DataTarget.LoadDump(dumpFullPath);

            if (_target.ClrVersions.Length == 0)
                throw new InvalidOperationException("Haven't found relevant CLR versions for the dump, cannot continue with the import");

            _runtime = _target.CreateRuntime();

            var allStrings = _runtime.Heap.EnumerateObjects()
                                .Where(x => x.Type.Name == "System.String")
                                .ToList();

            //TODO: write code to investigate the dumps
        }
    }
}
