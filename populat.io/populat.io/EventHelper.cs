using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace populat.io
{
    class EventHelper
    {
        private int chanceDisease;
        private int chanceWeather;
        private int chanceWar;
        private int chanceHigherImmigration;
        private int chanceBetterMedication;
        private int chanceHigherAverageIncome;


        public EventHelper(int chanceDisease, int chanceWeather, int chanceWar, int chanceHigherImmigration,
            int chanceBetterMedication, int chanceHigherAverageIncome)
        {
            this.chanceDisease = chanceDisease;
            this.chanceWeather = chanceWeather;
            this.chanceWar = chanceWar;
            this.chanceHigherImmigration = chanceHigherImmigration;
            this.chanceBetterMedication = chanceBetterMedication;
            this.chanceHigherAverageIncome = chanceHigherAverageIncome;
        }


        public List<string> SimulateEvents(Population p)
        {
            List<string> outcomes = new List<string>();
            int chance = StaticRandom.Instance.Next(1, 101);
            if (chance < chanceDisease)
            {
                outcomes.Add(SimulateDisease(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            if (chance < chanceWeather)
            {
                outcomes.Add(SimulateWeather(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            if (chance < chanceWar)
            {
                outcomes.Add(SimulateWar(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            if (chance < chanceHigherImmigration)
            {
                outcomes.Add(SimulateHigherImmigration(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            if (chance < chanceBetterMedication)
            {
                outcomes.Add(SimulateBetterMedication(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            if (chance < chanceHigherAverageIncome)
            {
                outcomes.Add(SimulateHigherAverageIncome(p));
            }
            return outcomes;
        }

        private string SimulateDisease(Population p)
        {
            double deathPercentage = StaticRandom.Instance.Next(10, 30);
            double emigrationRate = StaticRandom.Instance.Next(5, 10);
            double immmigrationRate = StaticRandom.Instance.Next(5, 10);
            double deathRate = StaticRandom.Instance.Next(10, 30);
            p.PopulationNr -= p.PopulationNr * deathPercentage * 0.01;
            p.EmigrationRate += p.EmigrationRate * emigrationRate * 0.01;
            p.ImmigrationRate -= p.ImmigrationRate * immmigrationRate * 0.01;
            p.DeathRate += p.DeathRate * deathRate * 0.01;
            return "Disease outbreak in " + p.Year + " killed " + deathPercentage + "%";
        }

        private string SimulateWeather(Population p)
        {
            double deathPercentage = StaticRandom.Instance.Next(5, 10);
            double emigrationRate = StaticRandom.Instance.Next(5, 10);
            double immmigrationRate = StaticRandom.Instance.Next(5, 10);
            double deathRate = StaticRandom.Instance.Next(5, 15);
            p.PopulationNr -= p.PopulationNr * deathPercentage * 0.01;
            p.EmigrationRate += p.EmigrationRate * emigrationRate * 0.01;
            p.ImmigrationRate -= p.ImmigrationRate * immmigrationRate * 0.01;
            p.DeathRate += p.DeathRate * deathRate * 0.01;
            return "Bad weather event in " + p.Year + " killed " + deathPercentage + "%";
        }

        private string SimulateWar(Population p)
        {
            double deathPercentage = StaticRandom.Instance.Next(10, 40);
            double emigrationRate = StaticRandom.Instance.Next(15, 30);
            double immmigrationRate = StaticRandom.Instance.Next(15, 30);
            double birthRate = StaticRandom.Instance.Next(5, 15);
            double deathRate = StaticRandom.Instance.Next(15, 45);
            double maleRate = StaticRandom.Instance.Next(5, 15);
            p.PopulationNr -= p.PopulationNr * deathPercentage * 0.01;
            p.EmigrationRate += p.EmigrationRate * emigrationRate * 0.01;
            p.ImmigrationRate -= p.ImmigrationRate * immmigrationRate * 0.01;
            p.BirthRate += p.BirthRate * birthRate * 0.01;
            p.DeathRate += p.DeathRate * deathRate * 0.01;
            p.MaleRate -= p.MaleRate * maleRate * 0.01;
            return "War in " + p.Year + " killed " + deathPercentage + "%";
        }

        private string SimulateHigherImmigration(Population p)
        {
            double immigrationRate = StaticRandom.Instance.Next(5, 20);
            double emigrationRate = StaticRandom.Instance.Next(5, 10);
            double birthRate = StaticRandom.Instance.Next(10, 15);
            double deathRate = StaticRandom.Instance.Next(5, 15);
            p.PopulationNr += p.PopulationNr * immigrationRate * 0.01;
            p.EmigrationRate -= p.EmigrationRate * emigrationRate * 0.01;
            p.ImmigrationRate += p.ImmigrationRate * immigrationRate * 0.01;
            p.BirthRate += p.BirthRate * birthRate * 0.01;
            p.DeathRate += p.DeathRate * deathRate * 0.01;
            return "Spike in immigration in " + p.Year + ". Population increased with " + immigrationRate + "%";
        }

        private string SimulateBetterMedication(Population p)
        {
            double populationIncreaseRate = StaticRandom.Instance.Next(5, 25);
            double immigrationRate = StaticRandom.Instance.Next(5, 20);
            double emigrationRate = StaticRandom.Instance.Next(5, 10);
            double birthRate = StaticRandom.Instance.Next(5, 15);
            double deathRate = StaticRandom.Instance.Next(25, 45);
            p.PopulationNr += p.PopulationNr * populationIncreaseRate * 0.01;
            p.EmigrationRate -= p.EmigrationRate * emigrationRate * 0.01;
            p.ImmigrationRate += p.ImmigrationRate * immigrationRate * 0.01;
            p.BirthRate += p.BirthRate * birthRate * 0.01;
            p.DeathRate -= p.DeathRate * deathRate * 0.01;
            return "Improvements in medication in " + p.Year + ". Population increased with " + populationIncreaseRate + "%";
        }
        private string SimulateHigherAverageIncome(Population p)
        {
            double populationIncreaseRate = StaticRandom.Instance.Next(5, 10);
            double immigrationRate = StaticRandom.Instance.Next(5, 20);
            double emigrationRate = StaticRandom.Instance.Next(5, 10);
            double birthRate = StaticRandom.Instance.Next(5, 15);
            double deathRate = StaticRandom.Instance.Next(10, 15);
            p.PopulationNr += p.PopulationNr * populationIncreaseRate * 0.01;
            p.EmigrationRate -= p.EmigrationRate * emigrationRate * 0.01;
            p.ImmigrationRate += p.ImmigrationRate * immigrationRate * 0.01;
            p.BirthRate += p.BirthRate * birthRate * 0.01;
            p.DeathRate -= p.DeathRate * deathRate * 0.01;
            return "Average income increased in " + p.Year + ". Population increased with " + populationIncreaseRate + "%";
        }
    }
}
