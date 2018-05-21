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

        public EventHelper(bool allowDisease, bool allowWeather, bool allowWar)
        {
            this.allowDisease = allowDisease;
            this.allowWeather = allowWeather;
            this.allowWar = allowWar;
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
    }
}
