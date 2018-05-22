using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace populat.io
{
    class EventHelper
    {
        private bool allowDisease;
        private bool allowWeather;
        private bool allowWar;
        private bool allowHigherImmigration;
        private bool allowBetterMedication;
        private bool allowHigherAverageIncome;

        public EventHelper(bool allowDisease, bool allowWeather, bool allowWar, bool allowHigherImmigration, 
            bool allowBetterMedication, bool allowHigherAverageIncome)
        {
            this.allowDisease = allowDisease;
            this.allowWeather = allowWeather;
            this.allowWar = allowWar;
            this.allowHigherImmigration = allowHigherImmigration;
            this.allowBetterMedication = allowBetterMedication;
            this.allowHigherAverageIncome = allowHigherAverageIncome;
        }

        public List<string> SimulateEvents(Population p)
        {
            List<string> outcomes = new List<string>();
            int chance = StaticRandom.Instance.Next(1, 101);
            // 10% for disease to occur
            if (allowDisease && chance < 10)
            {
                outcomes.Add(SimulateDisease(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            // 20% for weather event to occur
            if (allowWeather && chance < 20)
            {
                outcomes.Add(SimulateWeather(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            // 5% for war to occur
            if (allowWar && chance < 5)
            {
                outcomes.Add(SimulateWar(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            // 10% for medication to improve
            if (allowHigherImmigration && chance < 10)
            {
                outcomes.Add(SimulateHigherImmigration(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            // 15% for medication to improve
            if (allowBetterMedication && chance < 15)
            {
                outcomes.Add(SimulateBetterMedication(p));
            }
            chance = StaticRandom.Instance.Next(1, 101);
            // 10% for income to increase
            if (allowBetterMedication && chance < 10)
            {
                outcomes.Add(SimulateHigherAverageIncome(p));
            }
            return outcomes;
        }

        private string SimulateDisease(Population p)
        {
            double deathPercentage = StaticRandom.Instance.Next(10, 30);
            p.PopulationNr -= p.PopulationNr * deathPercentage * 0.01;
            return "Disease outbreak in " + p.Year + " killed " + deathPercentage + "%";
        }

        private string SimulateWeather(Population p)
        {
            double deathPercentage = StaticRandom.Instance.Next(5, 10);
            p.PopulationNr -= p.PopulationNr * deathPercentage * 0.01;
            return "Bad weather event in " + p.Year + " killed " + deathPercentage + "%";
        }

        private string SimulateWar(Population p)
        {
            double deathPercentage = StaticRandom.Instance.Next(10, 40);
            p.PopulationNr -= p.PopulationNr * deathPercentage * 0.01;
            return "War in " + p.Year + " killed " + deathPercentage + "%";
        }

        private string SimulateHigherImmigration(Population p)
        {
            double immigrationRate = StaticRandom.Instance.Next(5, 20);
            p.PopulationNr += p.PopulationNr * immigrationRate * 0.01;
            return "Spike in immigration in " + p.Year + ". Population increased with " + immigrationRate + "%";
        }

        private string SimulateBetterMedication(Population p)
        {
            double populationIncreaseRate = StaticRandom.Instance.Next(5, 25);
            p.PopulationNr += p.PopulationNr * populationIncreaseRate * 0.01;
            return "Improvements in medication in " + p.Year + ". Population increased with " + populationIncreaseRate + "%";
        }
        private string SimulateHigherAverageIncome(Population p)
        {
            double populationIncreaseRate = StaticRandom.Instance.Next(5, 10);
            p.PopulationNr += p.PopulationNr * populationIncreaseRate * 0.01;
            return "Average income increased in " + p.Year + ". Population increased with " + populationIncreaseRate + "%";
        }
    }
}
