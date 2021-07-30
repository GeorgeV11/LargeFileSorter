using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class BucketSortInteger
    {
        int[] _Array;
        int _MaxVal;
        public BucketSortInteger(int maxValue)
        {
            _MaxVal = maxValue;
            _Array = new int[maxValue];
            for (int i = 0; i < maxValue; i++)
                _Array[i] = 0;
        }
        public void Add(int val)
        {
            _Array[val]++;
        }
        public void ProcessSorted(Action<int> onSortedV)
        {
            for (int i = 0; i < _MaxVal; i++)
            {
                for (int j = 0; j < _Array[i]; j++)
                    onSortedV(i);
            }
        }

    }
}
