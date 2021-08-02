using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SortStrings;

namespace GenerateSortTest
{
    [TestClass]
    public class SorterGeneratorTest
    {
        string _FileName;
        string _OutputFileName = null;


        [TestInitialize ]
        public void Start()
        {
            GenerateFile();
        }

        void   GenerateFile()
        {            
            var _SortStringHelper = new SortStrings.Helpers();
            _FileName = Path.GetTempFileName();
            _FileName = string.Format("{0}.csv", Path.GetFileNameWithoutExtension(_FileName));
            _SortStringHelper.LoadWordDictionary();
            _SortStringHelper.GenerateFile(_FileName, 1000000);         
        }

        [TestMethod]
        public void TGenerateFileCheck()
        {
            using (var streamReader = new StreamReader(_FileName))
            {
                var enumerate = Helper.StreamAsIEnumerable(streamReader);
                NumberStringLine value;
                foreach (var val in enumerate)
                    value = val;
            }
        }
        [TestMethod]
        public void TSortFile()
        {
            var sorter = new LargeFileSorter();
            _OutputFileName = string.Format("{0}_sorted.{1}",Path.GetFileNameWithoutExtension(_FileName),"csv");
            sorter.SortLargeFile(_FileName, _OutputFileName);            
            Assert.IsTrue(Helper.CheckIfFileSorted(_OutputFileName));
        }

        [TestCleanup ]
        public void CleanUp()
        {
            File.Delete(_FileName);
            if (_OutputFileName!=null)
                File.Delete(_OutputFileName);
        }        
    }
}
