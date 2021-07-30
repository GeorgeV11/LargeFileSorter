using SortStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileGenerator
{
    class Program
    {
        static void GenerateFile(string fileName, long size)
        {
                var h = new Helpers();
                h.LoadWordDictionary();
                h.GenerateFileSized(fileName, size);
        }

        const string _DefailtFileName = "generated.csv";
        const long _DefaultSize = 1000000000;
        const string _SizeKey = "/s:";

        static void PrintUsage()
        {
            Console.WriteLine("LargeFileGenerator generated.csv /s:10000000000");
        }
        static void Main(string[] args)
        {
            long size = _DefaultSize;
            var outputFileName = _DefailtFileName;
            if (args.Length>0)
            {
                List<string> arguments = args.ToList();
                var sizeKeyArg= arguments.Where(x => x.StartsWith(_SizeKey)).FirstOrDefault();
                if (sizeKeyArg != null)
                {
                    var sizeStr = sizeKeyArg.Substring(_SizeKey.Length, sizeKeyArg.Length - _SizeKey.Length);
                    if (!long.TryParse(sizeStr, out size))
                    {
                        Console.WriteLine("Invalid size argument format:{0}", sizeStr);
                        PrintUsage();
                        return;
                    }
                    arguments.Remove(sizeKeyArg);
                }
                if (arguments.Count > 0)
                    outputFileName = arguments[0];

            }
            GenerateFile(outputFileName, size);
        }
    }
}
