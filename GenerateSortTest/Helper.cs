using SortStrings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateSortTest
{
    public class Helper
    {
        public static bool CheckIfFileSorted(string path)
        {
            using (var streamReader= new StreamReader(path))
            {
                bool res = true;
                var fileReader = StreamAsIEnumerable(streamReader);
                var iEnum = fileReader.GetEnumerator();
                iEnum.MoveNext();
                var oldVal = iEnum.Current;
                var secondVal = oldVal;
                while (iEnum.MoveNext())
                {
                    secondVal = iEnum.Current;
                    res = !(oldVal > secondVal);
                    if (!res)
                        break;
                    oldVal = secondVal;
                }
                return res;
            }


        }
        
        public static IEnumerable<NumberStringLine> StreamAsIEnumerable(StreamReader stream)
        {
            if (stream != null)            
                while (!stream.EndOfStream)
                {
                    string ln = stream.ReadLine();
                    yield return (NumberStringLine.FromString(ln));
                }            
        }
    }
}
