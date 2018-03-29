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

        public City(string name, List<Population> data)
        {
            Name = name;
            PopulationThroughYears = data;
        }
    }
}
