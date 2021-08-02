using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class StringFirstLetterContainerHandler : BaseContanerHandler
    {
        
        public int CharIndex { get; private set; }
        public StringFirstLetterContainerHandler(int charIndex, string tmpDir, string filePrefix)
        {
            _Files = new List<PartFile>(256);
            CharIndex = charIndex;
            // create special container that contains strings that already finished 
            _Files.Add(new PartFile(tmpDir, filePrefix, 0.ToString(), EContainerMode.StringFinished, charIndex+1)) ;
            //other containers
            for (int i = 1; i < 256; i++)
                _Files.Add(new PartFile(tmpDir, filePrefix, i.ToString(), EContainerMode.StringSpilt, charIndex + 1));
        }

        public override PartFile GetContainer(string line)
        {
            var dotPosition = line.IndexOf('.');
            int ind = dotPosition + 1 + CharIndex;
            char c;
            int intCharVal = 0;
            if (ind < line.Length)
            {
                c = line[ind];
                intCharVal = Char.ConvertToUtf32(line, ind);
            }
            if (intCharVal >= 256)
                throw new NotImplementedException();
            return _Files[intCharVal];
        }
        
    }
}
