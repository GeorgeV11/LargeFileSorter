using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class LargeFileSorter
    {
        public const long DefaultMaxFileSizeToSortInMemory=100*1024*1024;
        public int BucketSortRecomenedDigits { get; set; } = 3;
        public long MaxFileSizeToSortInMemory { get; set; }

        public  void SortLargeFile(string inputFilePath, string outputFilePath, long _MaxFileSizeToSortInMemory= DefaultMaxFileSizeToSortInMemory)
        {
            MaxFileSizeToSortInMemory = _MaxFileSizeToSortInMemory;
            BucketSortRecomenedDigits = (int) Math.Floor(Math.Log10(_MaxFileSizeToSortInMemory / sizeof(int)));            
            var info = new FileInfo(inputFilePath);
            //If file size is less  MaxFileSize  we can just  sort file's content
            if (info.Length < MaxFileSizeToSortInMemory)
                SortSmallPart(inputFilePath, outputFilePath);         
            else
            {               
                var containers=SplitFileSt(inputFilePath, 0);
                if (containers!=null)
                    MergeSt(outputFilePath, containers, true);
            }
        }
        protected  static  void MergeSt(string outputFileName, IList<PartFile> usedFiles , bool deleteSource)
        {            
            using (var output = File.Create(outputFileName))
            {
                foreach (var file in usedFiles)
                {
                    file.Close();                 
                    {
                        CopyFile(file.FilePath, output);
                        if (deleteSource)
                            File.Delete(file.FilePath);
                    }
                }
            }
        }        
        protected static void SortSmallPart(string inputFilePath, string outputFile)
        {
            var h = new Helpers();
            var data = h.LoadData(inputFilePath);
            data.Sort();
            var lst =data.Select(x => x.ToString()).ToList();
            File.WriteAllLines(outputFile, lst);
        }




        protected static void  ProcessBucketSort(string inputFilePath,int startPosition,int totalDigits)
        {
            var size = totalDigits - startPosition;
            int val10=1;
            for (int i = 0; i < size; i++)
                val10 *= 10;
            var sorter = new BucketSortInteger(val10);
            int index = 0;
            string restOfDigits = string.Empty;
            string stringPart = string.Empty;
            using (var inputReader = new StreamReader(inputFilePath))
            {
                while (!inputReader.EndOfStream)
                {
                    string line = inputReader.ReadLine();
                    if (index == 0)
                    {
                        var dotInd = line.IndexOf('.');
                        stringPart = line.Substring(dotInd + 1);
                        restOfDigits = line.Substring(0, startPosition);
                    }
                    int startVal = 1;
                    int integerPart = 0;
                    for(int charPos= totalDigits - 1; charPos>= startPosition; charPos--)
                    {
                        var c = line[charPos];
                        int val = (int)Char.GetNumericValue(c);
                        integerPart += startVal * val;                        
                        startVal *= 10;
                    }
                    sorter.Add(integerPart);
                    index++;
                }
                inputReader.Close();
            }            
            using (var outputWriter = new StreamWriter(inputFilePath))
            {
                sorter.ProcessSorted( delegate (int sorted)
                {                   
                    var line=string.Format("{0}{1:D"+ size + "}.{2}", restOfDigits, sorted, stringPart);
                    outputWriter.WriteLine(line);
                });
            }
        }

        protected  IList<PartFile> SplitFileSt(string inputFilePath, int charIndex, PartFile parrentContainer=null            )
        {
            var tmpDir = Path.GetDirectoryName(inputFilePath);
            IList<PartFile> usedFiles = null;
            IContanerHandler handler = ContainerHandlerFactory.CreateContainerHandler(parrentContainer, tmpDir, charIndex);
            if (handler == null)
                ProcessBucketSort(inputFilePath, parrentContainer.NextCharPossition, parrentContainer.NumberOfDigits);
            else
            {
                using (var inputReader = new StreamReader(inputFilePath))
                {
                    while (!inputReader.EndOfStream)
                    {
                        string line = inputReader.ReadLine();
                        var container = handler.GetContainer(line);
                        container.WriteLine(line);
                    }
                }
                //
                handler.CloseAllFiles();
                usedFiles = handler.GetUsedContainers();
                foreach (var file in usedFiles)
                {
                    {
                        if (file.Size > MaxFileSizeToSortInMemory)
                        {
                            var sply = SplitFileSt(file.FilePath, charIndex + 1, file);
                             if (sply != null)
                                {
                                    File.Delete(file.FilePath);
                                    MergeSt(file.FilePath, sply, true);
                                }
                        }
                        else
                            SortSmallPart(file.FilePath, file.FilePath);
                    }
                }
            }
            return usedFiles;
        }

        protected static void CopyFile(string fileName, FileStream output)
        {
            using (var file = File.OpenRead(fileName))
            {
                file.CopyTo(output);
            }
        }
    }
}
