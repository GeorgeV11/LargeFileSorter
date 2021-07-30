using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class NumberOfDigitSplitContainerHandler : BaseContanerHandler
    {
         const int _MaxDigits = 9;
        public NumberOfDigitSplitContainerHandler(string tmpDir, string filePrefix)
        {
            _Files = new List<PartFile>(_MaxDigits);            
            for (int i = 0; i < _MaxDigits; i++)
                _Files.Add(new PartFile(tmpDir, filePrefix, string.Format("DigitN{0}", i) ,EContainerMode.DigitNumber,0,i));
        }
        public override PartFile GetContainer(string line)
        {
            var dotPosition = line.IndexOf('.');
            if (dotPosition > _MaxDigits)
            {
                throw new NotImplementedException();
            }
            return _Files[dotPosition];
        }
    }
}
