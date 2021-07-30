using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public  class NumberStringLine : IComparable, IComparable<NumberStringLine>
    {
        public string StringPart { get; set; }
        public int NumberPart { get; set; }
        public static NumberStringLine FromString(string line)
        {
            var splt = line.Split(new char[] { '.' },2);
            var str = splt.Length>1?splt[1]:string.Empty;
            return new NumberStringLine()
            { NumberPart = int.Parse(splt[0]), StringPart = str };
        }

        public int CompareTo(object obj)
        {
            int res = -1;
            var other = (obj as NumberStringLine);
            if (other != null)
                res = CompareTo(other);
            else 
              throw new NotImplementedException();
            return res;
        }

        public int CompareTo(NumberStringLine other)
        {
            int res = this.StringPart.CompareTo(other.StringPart);
            if (res == 0)
                res = NumberPart.CompareTo(other.NumberPart);
            return res;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", NumberPart, StringPart);
        }
        public string ToStringAlt()
        {
            var strNum = NumberPart.ToString();
            var b = new StringBuilder(strNum);
            b.Append(".").Append(StringPart);
            return b.ToString();
        }
    }
}
