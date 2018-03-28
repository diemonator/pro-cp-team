using System;
using System.IO;
using System.Collections.Generic;

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

        public City LoadFromFile()
        {
            City temp = null;
            FileStream fs = null;
            StreamReader sr = null;
            List<Population> populations = new List<Population>();
            try
            {
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    populations.Add(PopulationFromString(sr.ReadLine()));
                }
                temp = new City(SafeFileName,populations);
            }
            catch (IOException e)
            {
                throw e;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
            return temp;
        }

        private Population PopulationFromString(string line)
        {
            Population temp = null;
            try
            {
                string[] words = line.Split(',');
                int year = Convert.ToInt32(words[0]);
                decimal deathRate = Convert.ToDecimal(words[1]);
                decimal birthRate = Convert.ToDecimal(words[2]);
                decimal immigrationRate = Convert.ToDecimal(words[3]);
                decimal emigrationRate = Convert.ToDecimal(words[4]);
                decimal populationNumber = Convert.ToDecimal(words[5]);
                double avrageAge = Convert.ToDouble(words[6]);
                decimal maleRate = Convert.ToDecimal(words[7]);
                decimal femaleRate = Convert.ToDecimal(words[8]);
                decimal growthRate = Convert.ToDecimal(words[9]);
                decimal age0_17 = Convert.ToDecimal(words[10]);
                decimal age18_34 = Convert.ToDecimal(words[11]);
                decimal age35_54 = Convert.ToDecimal(words[12]);
                decimal age55_up = Convert.ToDecimal(words[13]);
                temp = new Population(year, deathRate, birthRate, immigrationRate, emigrationRate, populationNumber, avrageAge, maleRate, femaleRate, growthRate, age0_17,age18_34,age35_54,age55_up);
                return temp;
            }
            catch (FormatException e)
            {
                throw e;
            }
        }
    }
}