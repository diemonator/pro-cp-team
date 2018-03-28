using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace populat.io
{
    class Population
    {
        public int Year { get; set; }
        public double DeathRate { get; set; }
        public double BirthRate { get; set; }
        public double ImmigrationRate { get; set; }
        public double EmigrationRate { get; set; }
        public double PopulationNr { get; set; }
        public double AverageAge { get; set; }
        public double MaleRate { get; set; }
        public double FemaleRate { get; set; }
        public double GrowthRate { get; set; }
        public double Age0_17 { get; set; }
        public double Age18_34 { get; set; }
        public double Age35_54 { get; set; }
        public double Age55_up { get; set; }


        public Population(int year, double deathRate, double birthRate, double immigrationRate, double emigrationRate, double populationNr, double averageAge, double maleRate, double femaleRate, double growthRate, double kids, double adults, double older, double elderly)
        {
            Year = year;
            DeathRate = deathRate;
            BirthRate = birthRate;
            ImmigrationRate = immigrationRate;
            EmigrationRate = emigrationRate;
            PopulationNr = populationNr;
            AverageAge = averageAge;
            MaleRate = maleRate;
            FemaleRate = femaleRate;
            GrowthRate = growthRate;
            Age0_17 = kids;
            Age18_34 = adults;
            Age35_54 = older;
            Age55_up = elderly;
        }

        public Population()
        {

        }
    }
}
