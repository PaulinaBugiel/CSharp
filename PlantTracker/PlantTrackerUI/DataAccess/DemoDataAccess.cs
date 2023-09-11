using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.DataAccess
{
    public class DemoDataAccess : IDataAccess
    {
        private List<WateringSystem> wateringSystems = new List<WateringSystem>()
        {
            new WateringSystem() { Id = 0, Name = "None" },
            new WateringSystem() { Id = 1, Name = "Ręczny" },
            new WateringSystem() { Id = 2, Name = "Hydroponika" },
            new WateringSystem() { Id = 3, Name = "Automatyczny" },
            new WateringSystem() { Id = 4, Name = "Deszcz" }
        };

        private List<PlantContainer> containers = new List<PlantContainer>()
        {
            new PlantContainer() { Id = 0, Name = "None", Capacity = 0.0f },
            new PlantContainer() { Id = 1, Name = "Doniczka", Capacity = 0.0f },
            new PlantContainer() { Id = 2, Name = "Słoik", Capacity = 0.0f },
            new PlantContainer() { Id = 3, Name = "Butelka", Capacity = 0.0f }
        };

        private List<PlantPosition> plantPositions = new List<PlantPosition>()
        {
            new PlantPosition() { Id = 0, Name = "None", Exposition = SunExposition.Unknown},
            new PlantPosition() { Id = 1, Name = "Parapet w Kuchni", Exposition = SunExposition.SouthEast},
            new PlantPosition() { Id = 2, Name = "Parapet w Salonie", Exposition = SunExposition.NorthWest},
            new PlantPosition() { Id = 3, Name = "Balkon", Exposition = SunExposition.SouthEast},
        };

        private ObservableCollection<PlantType> plantTypes = new ObservableCollection<PlantType>()
        {
            new PlantType() { Id = 0, Name = "None" },
            new PlantType() { Id = 1, Name = "Ziele" },
            new PlantType() { Id = 2, Name = "Warzywo" },
            new PlantType() { Id = 3, Name = "Jadalne" },
            new PlantType() { Id = 4, Name = "Kwiat" },
            new PlantType() { Id = 5, Name = "Ozdobne" }
        };

        public List<WateringSystem> WateringSystem_GetAll()
        {
            return wateringSystems;
        }

        public void WateringSystem_InsertOne(WateringSystem model)
        {
            wateringSystems.Add(model);
        }

        public List<PlantContainer> Container_GetAll()
        {
            return containers;
        }

        public List<PlantPosition> PlantPosition_GetAll()
        {
            return plantPositions;
        }


        public void PlantType_InsertOne(PlantType model)
        {
            plantTypes.Add(model);
        }
        public ObservableCollection<PlantType> PlantType_GetAll()
        {
            return plantTypes;
        }

        public List<Plant> Plants_GetAll()
        {
            List<Plant> ret = new List<Plant>
            {
                // add a few Plants
                new Plant()
                {
                    Name = "Bazylia",
                    PlantTypes = plantTypes.Where(x => x.Name == "Ziele" || x.Name == "Jadalne" || x.Name == "Ozdobne" || x.Name == "Warzywo").ToList(),
                    PlantingDate = new DateTime(2023, 09, 16),
                    Containers = containers.Where(x => x.Name == "Słoik").ToList(),
                    WateringSystems = wateringSystems.Where(x => x.Name == "Hydroponika" || x.Name == "Ręczny").ToList(),
                    Position = plantPositions.Where(x => x.Name == "Parapet").FirstOrDefault()
                },
                new Plant()
                {
                    Name = "Pomidor",
                    PlantTypes = plantTypes.Where(x => x.Name == "Warzywo" || x.Name == "Jadalne").ToList(),
                    PlantingDate = new DateTime(2023, 09, 16),
                    Containers = containers.Where(x => x.Name == "Doniczka").ToList(),
                    WateringSystems = wateringSystems.Where(x => x.Name == "Ręczny").ToList(),
                    Position = plantPositions.Where(x => x.Name == "Parapet w Kuchni").FirstOrDefault()
                },
                new Plant()
                {
                    Name = "Kaktus",
                    PlantTypes = plantTypes.Where(x => x.Name == "Ozdobne").ToList(),
                    PlantingDate = new DateTime(2023, 09, 16),
                    Containers = containers.Where(x => x.Name == "Słoik").ToList(),
                    WateringSystems = wateringSystems.Where(x => x.Name == "Ręczny").ToList(),
                    Position = plantPositions.Where(x => x.Name == "Balkon").FirstOrDefault()
                }
            };
            return ret;
        }
    }
}
