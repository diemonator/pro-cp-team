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
        public decimal DeathRate { get; set; }
        public decimal BirthRate { get; set; }
        public decimal ImmigrationRate { get; set; }
        public decimal EmigrationRate { get; set; }
        public decimal PopulationNr { get; set; }
        public double AvrageAge { get; set; }
        public decimal MaleRate { get; set; }
        public decimal FemaleRate { get; set; }
        public decimal GrowthRate { get; set; }
        public decimal Age0_17 { get; set; }
        public decimal Age18_34 { get; set; }
        public decimal Age35_54 { get; set; }
        public decimal Age55_up { get; set; }


        public Population(int year, decimal deathRate, decimal birthRate, decimal immigrationRate, decimal emigrationRate, decimal populationNr, double avrageAge, decimal maleRate, decimal femaleRate, decimal growthRate, decimal kids, decimal adults, decimal older, decimal elderly)
        {
            Year = year;
            DeathRate = deathRate;
            BirthRate = birthRate;
            ImmigrationRate = immigrationRate;
            EmigrationRate = emigrationRate;
            PopulationNr = populationNr;
            AvrageAge = avrageAge;
            MaleRate = maleRate;
            FemaleRate = femaleRate;
            GrowthRate = growthRate;
            Age0_17 = kids;
            Age18_34 = adults;
            Age35_54 = older;
            Age55_up = elderly;
        }
    }
}
