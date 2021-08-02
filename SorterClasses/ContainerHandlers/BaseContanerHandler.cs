using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public abstract class BaseContanerHandler : IContanerHandler
    {
        protected List<PartFile> _Files;
        public void CloseAllFiles()
        {
            foreach (var f in _Files)
                f.Close();
        }

        public abstract  PartFile GetContainer(string value);
        
        public IList<PartFile> GetUsedContainers()
        {
            var res = _Files.Where(x => x.WasUsed).ToList();
            return res;
        }
        
    }
}
