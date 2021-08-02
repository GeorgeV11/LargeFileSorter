using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public interface IContanerHandler
    {
        PartFile GetContainer(string value);
        IList<PartFile> GetUsedContainers();
        void CloseAllFiles();
    }
}
