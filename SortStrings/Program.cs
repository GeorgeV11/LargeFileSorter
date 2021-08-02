using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    class Program
    {

        static void SortFile(string input, string output, long memorySize)
        {
            TickCounter tLoad;
            using (tLoad = new TickCounter())
            {
                var lSorter = new LargeFileSorter();
                lSorter.SortLargeFile(input, output, memorySize);
            }
            var tms = TimeSpan.FromMilliseconds(tLoad.Result);
            Console.WriteLine(tms);
        }
        const string _MemKey = "/mem:";
        const string _Info = "/?";

        static void PrintUsage()
        {
            Console.WriteLine("SortStrings generated.csv sorted.csv  /mem:2000000000");
        }

        static void Main(string[] args)
        {
            string path;            
            string pathOut ;
            long memorySize =   200*1024*1024; 
            if (args.Length > 0)
            {
                List<string> arguments = args.ToList();
                if (arguments.Contains(_Info))
                {
                    PrintUsage();
                    return;
                }
                var sizeKeyArg = arguments.Where(x => x.StartsWith(_MemKey)).FirstOrDefault();
                if (sizeKeyArg != null)
                {
                    var sizeStr = sizeKeyArg.Substring(_MemKey.Length, sizeKeyArg.Length - _MemKey.Length);
                    if (!long.TryParse(sizeStr, out memorySize))
                    {
                        Console.WriteLine("Invalid size argument format:{0}", sizeStr);
                        PrintUsage();
                        return;
                    }
                    arguments.Remove(sizeKeyArg);
                }
                if (arguments.Count >= 2)
                {
                    path = arguments[0];
                    pathOut = arguments[1];

                }
                else
                {
                    PrintUsage();
                    return;
                }
            }
            else
            {
                PrintUsage();
                return;
            }
            SortFile(path, pathOut, memorySize);            
            Console.ReadKey();                                     
        }
    }
}
