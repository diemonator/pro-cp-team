using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;

namespace populat.io
{
    class CSVHelper
    {
        private string safeFileName;

        public string SafeFileName
        {
            get { return safeFileName; }
            set
            {
                string[] name = value.Split('.');
                safeFileName = name[0];
            }
        }
        public string FileName { get; private set; }

        public CSVHelper()
        {
            FileName = null;
        }

        public CSVHelper(string fileName, string safeFileName)
        {
            FileName = fileName;
            SafeFileName = safeFileName;
        }

        public void WriteFile()
        {
            var records = new List<Population>();
            records.Add(new Population(2015, 0.00878, 0.0109, 0.138, 0, 821.752, 37.5, 0.494, 0.506, 0.0064, 0.1786, 0.3065, 0.2862, 0.2287));
            using (TextWriter textWriter = File.CreateText(FileName)) {
                var csv = new CsvWriter(textWriter);
                csv.WriteRecords(records);
            }
        }

        public City ReadFile()
        {
            using (TextReader fileReader = File.OpenText(FileName))
            {
                var csv = new CsvReader(fileReader);
                var records = csv.EnumerateRecords(new Population());
                List<Population> temp = new List<Population>((IEnumerable<Population>)records);
                City amsterdam = new City(SafeFileName, temp);
                return amsterdam;
            }
           
        }

    }
}