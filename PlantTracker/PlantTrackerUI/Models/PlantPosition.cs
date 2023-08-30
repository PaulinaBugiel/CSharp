using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Models
{
    public class PlantPosition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SunExposition Exposition { get; set; }
    }

    public enum SunExposition
    {
        Unknown,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
}
