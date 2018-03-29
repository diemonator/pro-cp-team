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

        public void WriteFile(City city)
        {
            var records = city.PopulationThroughYears;
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
                var records = csv.GetRecords<Population>();
                List<Population> temp = new List<Population>(records);
                return new City(SafeFileName, temp);
            }
           
        }

    }
}