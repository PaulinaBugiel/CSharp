using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Models
{
    public class Plant : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ObservableCollection<PlantType> PlantTypes { get; set; } = new();
        public DateTime PlantingDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public List<PlantContainer> Containers { get; set; } = new();
        public List<WateringSystem> WateringSystems { get; set; } = new();
        public PlantPosition? Position { get; set; } = new();

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
                return PlantTypes.ToString(',');
            }
        }

        public string ContainersString
        {
            get
            {
                return Containers.ToString(',');
            }
        }

    }
}
