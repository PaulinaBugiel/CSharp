using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Models
{
    public class PlantContainer : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Capacity { get; set; }
    }
}
