using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace populat.io
{
    class City
    {
        public string Name { get; set; }
        public List<Population> PopulationThroughYears { get; set; }
        public int LastRecord { get; set; }
        public double PopulationRate { get;  set;}

        public City(string name, List<Population> data)
        {
            Name = name;
            PopulationThroughYears = data;
            LastRecord = PopulationThroughYears.Count();

            int index1 = LastRecord - 1;
            int index2 = LastRecord - 2;
            Population preBaseYear = PopulationThroughYears[index2];
            Population baseYear = PopulationThroughYears[index1];

            PopulationRate = (baseYear.PopulationNr * 100 / preBaseYear.PopulationNr) / 100;
        }

        public List<string> Simulate(int year, EventHelper eh)
        {
            double i = 0;
            double BirthDifference;
            double DeathDifference;
            int index = PopulationThroughYears.Count() - 2;
            Population lastYear = PopulationThroughYears.Last();
            Population lastlastYear = PopulationThroughYears[index];

            BirthDifference = Difference(lastYear.BirthRate, lastlastYear.BirthRate);
            DeathDifference = Difference(lastYear.DeathRate, lastlastYear.DeathRate);

            if (BirthDifference > DeathDifference) i = 1*0.01;
            else if (BirthDifference < DeathDifference) i = -1*0.01;

            Population p = new Population
            {
                Year = year,
                //PopulationNr = lastYear.PopulationNr * (1 + ((lastYear.BirthRate - lastYear.DeathRate)*10)),
                //BirthRate = lastYear.BirthRate + GetRandomNumber2(),

                //Here we multiply last years rates by a random value between 0.99 and 1.01
                BirthRate = lastYear.BirthRate * GetRandomNumber(),
                DeathRate = lastYear.DeathRate * GetRandomNumber(),
                

                //depending who's got the biggest increase from death/birth there might be 0.01 removed or added

                PopulationNr = lastYear.PopulationNr * (PopulationRate * GetRandomNumber2() + i),

                MaleRate = lastYear.MaleRate * GetRandomNumber(),
                FemaleRate = lastYear.FemaleRate * GetRandomNumber(),
                EmigrationRate = lastYear.EmigrationRate * GetRandomNumber(),
                ImmigrationRate = lastYear.ImmigrationRate * GetRandomNumber(),
                AverageAge = lastYear.AverageAge * GetRandomNumber(),
                Age0_17 = lastYear.Age0_17 * GetRandomNumber(),
                Age18_34 = lastYear.Age18_34 * GetRandomNumber(),
                Age35_54 = lastYear.Age35_54 * GetRandomNumber(),
                Age55_up = lastYear.Age55_up * GetRandomNumber()
            };
            //moved growth rate out of the initialization since it needs this years population and lasts to calculate the difference
            p.GrowthRate = 100 * p.PopulationNr / lastYear.PopulationNr;          
            PopulationThroughYears.Add(p);
            return eh.SimulateEvents(p);   
        }

        private double GetRandomNumber()
        {
            return StaticRandom.Instance.Next(98, 102) * 0.01;
        }

        private double GetRandomNumber2()
        {
            return StaticRandom.Instance.Next(99, 101) * 0.01;
        }

        private double SteadyTrend(double lastYear, double lastLastYear)
        {
            double rnd;
            double helper = lastYear * 100 / lastLastYear;
            helper = helper / 100;
            rnd = StaticRandom.Instance.Next(99, 102) * 0.01;
            helper = rnd* helper;
            return helper;
        }

        private double Difference(double lastYear, double lastLastYear)
        {
            return (lastYear * 100 / lastLastYear);
        }
    }
}
