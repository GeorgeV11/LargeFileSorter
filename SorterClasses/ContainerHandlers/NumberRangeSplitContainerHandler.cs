using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class NumberRangeSplitContainerHandler : BaseContanerHandler
    {
        public int CharIndex { get; private set; }
        const int numberOfDToSplit = 2;
        public NumberRangeSplitContainerHandler(string tmpDir, string  filePrefix, int charIndex,int numberOfDigits)
        {
            CharIndex = charIndex;
           _Files = new List<PartFile>(10 * 10);            
            for (int i = 0; i < 100; i++)
                _Files.Add(new PartFile(tmpDir, filePrefix, i.ToString(),EContainerMode.DigitRange, charIndex + numberOfDToSplit, numberOfDigits));
        }
        public override PartFile GetContainer(string line)
        {
            int ind = CharIndex;
            char c;
            c = line[ind];
            int val10 = (int)Char.GetNumericValue(c);
            c = line[ind + 1];
            int val1 = (int)Char.GetNumericValue(c);
             var intCharVal = val10 * 10 + val1;
            return _Files[intCharVal];
        }
    }
}
