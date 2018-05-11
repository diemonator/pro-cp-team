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
        public string Name { get; set; }
        public List<Population> PopulationThroughYears { get; set; }
        public int LastRecord { get; set; }

        public City(string name, List<Population> data)
        {
            Name = name;
            PopulationThroughYears = data;
            rnd = new Random();
            LastRecord = PopulationThroughYears.Count();
        }

        public void Simulate(int year)
        {                 
            Population lastYear = PopulationThroughYears.Last();
            Population p = new Population
            {
                Year = year,
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

        private double GetRandomNumber()
        {
            return rnd.NextDouble() * (0.05 - (-0.05)) + (-0.05);
        }
    }
}
