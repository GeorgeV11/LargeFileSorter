using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SortStrings
{
    public class Helpers
    {
        public static string  _DictionaryFileName="english-word-list-total.csv";
        public static int MaxnumberOfWords = 10;
        public static int _MinIntGenerate = 1;
        public static int _MaxIntGenerate = 999999;
        public List<string> EnglishWords = new List<string>();
        Random rand = new Random();
        public  void GenerateFile(string path, int recordCount)
        {
            using (var fileWr = new StreamWriter(path))
            {
                for (int i = 0; i < recordCount; i++)
                {
                    fileWr.WriteLine(GenerateItem().ToString());
                }
            }
        }
        
        public void GenerateFileSized(string path, Int64 desizedSize)
        {
            using (var fileWr = new StreamWriter(path))
            {
                int ind = 0;
                Int64 currentSz = 0;
                while (currentSz < desizedSize)
                {
                    var gn = GenerateItem().ToStringAlt();
                    currentSz += gn.Length + 2;
                    fileWr.WriteLine(gn);
                    if (ind % 10000000 == 0)
                        Console.WriteLine(string.Format("index={0}, sz={1}",ind,currentSz));
                    ind++;
                }
            }
        }

        public void LoadWordDictionary(string file=null)
        {
            if (file == null)
                file = _DictionaryFileName;
            using (var wordsStream = new StreamReader(file))
            {
                wordsStream.ReadLine();
                while (!wordsStream.EndOfStream)
                {
                    EnglishWords.Add(wordsStream.ReadLine().Split(';')[1]);
                }
            }
        }

        public string GetFirstWord(string line)
        {
            var dotInd=line.IndexOf('.');
            int stringStart = dotInd + 1;
            var spaceInd=line.IndexOf(' ', stringStart);
            if (spaceInd == -1)
                spaceInd = line.Length;
            var res = line.Substring(dotInd + 1, spaceInd - stringStart);
            return res;

        }

        public void SplitByWord(string path)
        {
            var dict = new Dictionary<string, DictionaryData>();
            DictionaryData dictData;
            using (var fileRead = new StreamReader(path))
            {
                for (int ind = 0; 
                    ///ind < maxNumber && 
                    !fileRead.EndOfStream; ind++)
                {
                    var ln = fileRead.ReadLine();
                    var word = GetFirstWord(ln);
                    if  (!dict.TryGetValue(word, out dictData))
                    {
                        var fileName = word + ".srt";
                        dictData = new DictionaryData()
                        {
                            Count = 1,
                            Filename = fileName,
                            fileStream = new StreamWriter(fileName),
                            Size = ln.Length + 2                            
                        };
                        dict[word] = dictData;
                    }
                    else
                    {
                        dictData.Count++;
                        dictData.Size += ln.Length + 2;
                    }
                    dictData.fileStream.WriteLine(ln);
                }
                foreach (var val in dict.Values)
                    val.fileStream.Dispose();
                var lst= dict.Select(x => string.Format("{0}  -  Count: {1} Size {2}  Filename {3} ", x.Key, x.Value.Count, x.Value.Size, x.Key + ".srt")).ToList();                
                File.WriteAllLines("SavedLists.txt", lst);                
            }
        }

        public List<NumberStringLine> LoadData(string path, int  lstCapacity= 100 * 1000 * 1000)
        {
            var lst = new List<NumberStringLine>(lstCapacity);
            using (var fileRead = new StreamReader(path))
            {
             for(int ind =0;!fileRead.EndOfStream; ind++)
                {
                    var ln=fileRead.ReadLine();                    
                    lst.Add( NumberStringLine.FromString(ln));
                }
            }
            return lst;
        }

        public  NumberStringLine GenerateItem()
        {            
            int wordCount = rand.Next(1, MaxnumberOfWords);
            //var b = new StringBuilder();
            string strRes = null;
            for (int wordN = 0; wordN < wordCount; wordN++)
            {
                var rnd = rand.Next(0, EnglishWords.Count - 1);
                var curW = EnglishWords[rnd];
                strRes=string.Concat(strRes, curW, " ");                
            }
            var intGeneareted = rand.Next(_MinIntGenerate, _MaxIntGenerate);
            var ns = new NumberStringLine()
            {
                NumberPart = intGeneareted,
                StringPart = strRes
                //b.ToString()
            };
            return ns;
        }

        public class DictionaryData
        {
            public Int64 Count { get; set; }
            public Int64 Size { get; set; }
            public string  Filename { get; set; }
            public StreamWriter  fileStream { get; set; }
        }
    }
}
