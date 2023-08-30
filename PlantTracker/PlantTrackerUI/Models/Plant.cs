using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlantType> PlantTypes { get; set; }
        public DateTime PlantingDate { get; set; }
        public DateTime LastModification { get; set; }
        public List<PlantContainer> Containers { get; set; }
        public List<WateringSystem> WateringSystems { get; set; }
        public PlantPosition Position { get; set; }
        public string PlantingDateString
        {
            get
            {
                return PlantingDate.ToShortDateString();
            }
        }

        public string TypesString
        {
            get
            {
                string ret = "";
                foreach (var type in PlantTypes)
                {
                    ret += $"{type.Name}, ";
                }
                ret = ret.Trim(' ');
                ret = ret.Trim(',');
                return ret;
            }
        }

    }
}
