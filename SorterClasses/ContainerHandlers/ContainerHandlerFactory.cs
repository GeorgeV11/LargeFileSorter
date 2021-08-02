using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class ContainerHandlerFactory
    {
        public static IContanerHandler CreateContainerHandler( PartFile parrentContainer, string tempDir, int charIndex)
        {
            IContanerHandler res=null;
            bool digitRangeCreate = false;
            int digitIndex = 0;
            var filePrefix = parrentContainer == null ? string.Empty : parrentContainer.FullFName;
            if ((parrentContainer != null) && (parrentContainer.IsStringFinished))
                res = new NumberOfDigitSplitContainerHandler(tempDir, parrentContainer.FullFName);
            else
                if ((parrentContainer != null) && (parrentContainer.ContainerMode == EContainerMode.DigitNumber))
                {
                digitIndex=0;
                digitRangeCreate = true;
                }
            else
                if ((parrentContainer != null) && (parrentContainer.ContainerMode == EContainerMode.DigitRange))
                {
                digitRangeCreate = true;
                digitIndex = parrentContainer.NextCharPossition;
              }             
            else
                 res = new StringFirstLetterContainerHandler(charIndex, tempDir, filePrefix);                
            if (digitRangeCreate)
            {
                if ((parrentContainer.NumberOfDigits - digitIndex) <= 4)
                {
                    return null;
                }
                else
                 res = new NumberRangeSplitContainerHandler(tempDir, filePrefix, digitIndex, parrentContainer.NumberOfDigits);                
            }
            return res;
        }
    }
}
