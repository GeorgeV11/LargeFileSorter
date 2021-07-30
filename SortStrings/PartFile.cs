using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class PartFile : IDisposable
    {
        protected StreamWriter PartWriter;
        
        public PartFile()
        {

        }
       
        public PartFile(string directory,string prefix, string posfix, EContainerMode containerMode, int nextCharPosition, int numberOfDigits=0)
        {
            FullFName = string.Format("{0}_{1}", prefix, posfix);
            FilePath = String.Format("{0}\\{1}.srt",directory, FullFName);            
            FilePrefix = prefix;
            ContainerMode = containerMode;
            NextCharPossition = nextCharPosition;
            NumberOfDigits = numberOfDigits;
        }
        #region Properties
        public string FilePath { get; private set; }
        public string FilePrefix { get; private set; }
        public string FilePostfix { get; private set; }
        public string Directory { get; private set; }
        public string FullFName { get; private set; }
        public bool IsStringFinished{ get
            { return this.ContainerMode == EContainerMode.StringFinished; } }
        public EContainerMode ContainerMode { get; private set; }
        public int NextCharPossition { get; private set; }
        public int NumberOfDigits { get; private set; }

        public long Size
        {
            get {
                var info = new FileInfo(FilePath);
                return info.Length;
            }
        }

        public bool WasUsed
        {
            get; private set;
        } = false;
        #endregion
        public void WriteLine(string ln)
        {
            if (!WasUsed)
            {
                PartWriter = new StreamWriter(FilePath);
                WasUsed = true;
            }
            PartWriter.WriteLine(ln);
        }

        public void Dispose()
        {
            Close();
        }
        public void Close()
        {
            if (PartWriter != null)
                PartWriter.Close();
        }
    }
}
