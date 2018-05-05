using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace populat.io
{
    class City
    {
        private static Random rnd;
        private int lastRecord;
        public string Name { get; set; }
        public List<Population> PopulationThroughYears { get; set; }

        public City(string name, List<Population> data)
        {
            Name = name;
            PopulationThroughYears = data;
            rnd = new Random();
            lastRecord = PopulationThroughYears.Count();
        }

        public void Simulate(int year)
        {
            PopulationThroughYears.RemoveRange(lastRecord, PopulationThroughYears.Count() - lastRecord);
            for (int i = PopulationThroughYears.Last().Year + 1; i <= year; i++)
            {              
                Population lastYear = PopulationThroughYears.Last();
                Population p = new Population
                {
                    Year = i,
                    PopulationNr = lastYear.PopulationNr * (1 + (lastYear.BirthRate - lastYear.DeathRate)),
                    BirthRate = lastYear.BirthRate + (GetRandomNumber()),
                    DeathRate = lastYear.DeathRate + (GetRandomNumber()),
                    GrowthRate = lastYear.GrowthRate + (GetRandomNumber()),
                    MaleRate = lastYear.MaleRate + (GetRandomNumber()),
                    FemaleRate = lastYear.FemaleRate + (GetRandomNumber()),
                    EmigrationRate = lastYear.EmigrationRate + (GetRandomNumber()),
                    ImmigrationRate = lastYear.ImmigrationRate + (GetRandomNumber()),
                    AverageAge = lastYear.AverageAge + (GetRandomNumber()),
                    Age0_17 = lastYear.Age0_17 + (GetRandomNumber()),
                    Age18_34 = lastYear.Age18_34 + (GetRandomNumber()),
                    Age35_54 = lastYear.Age35_54 + (GetRandomNumber()),
                    Age55_up = lastYear.Age55_up + (GetRandomNumber())
                };
                PopulationThroughYears.Add(p);
            }
        }

        private double GetRandomNumber()
        {
            return rnd.NextDouble() * (0.05 - (-0.05)) + (-0.05);
        }
    }
}
