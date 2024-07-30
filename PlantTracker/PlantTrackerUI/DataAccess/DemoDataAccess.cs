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
        private static ObservableCollection<WateringSystem> wateringSystems = new ObservableCollection<WateringSystem>()
        {
            new WateringSystem() { Id = 0, Name = "None" },
            new WateringSystem() { Id = 1, Name = "Ręczny" },
            new WateringSystem() { Id = 2, Name = "Hydroponika" },
            new WateringSystem() { Id = 3, Name = "Automatyczny" },
            new WateringSystem() { Id = 4, Name = "Deszcz" }
        };

        private static ObservableCollection<PlantContainer> containers = new ObservableCollection<PlantContainer>()
        {
            new PlantContainer() { Id = 0, Name = "None", Capacity = 0.0f },
            new PlantContainer() { Id = 1, Name = "Doniczka", Capacity = 0.0f },
            new PlantContainer() { Id = 2, Name = "Słoik", Capacity = 0.0f },
            new PlantContainer() { Id = 3, Name = "Butelka", Capacity = 0.0f }
        };

        private static List<PlantPosition> plantPositions = new List<PlantPosition>()
        {
            new PlantPosition() { Id = 0, Name = "None", Exposition = SunExposition.Unknown},
            new PlantPosition() { Id = 1, Name = "Parapet w Kuchni", Exposition = SunExposition.SouthEast},
            new PlantPosition() { Id = 2, Name = "Parapet w Salonie", Exposition = SunExposition.NorthWest},
            new PlantPosition() { Id = 3, Name = "Balkon", Exposition = SunExposition.SouthEast},
        };

        private static ObservableCollection<PlantType> plantTypes = new ObservableCollection<PlantType>()
        {
            new PlantType() { Id = 0, Name = "None" },
            new PlantType() { Id = 1, Name = "Ziele" },
            new PlantType() { Id = 2, Name = "Warzywo" },
            new PlantType() { Id = 3, Name = "Jadalne" },
            new PlantType() { Id = 4, Name = "Kwiat" },
            new PlantType() { Id = 5, Name = "Ozdobne" }
        };

        private static ObservableCollection<Plant> plantsFull = new ObservableCollection<Plant>
            {
                // add a few Plants
                new Plant()
                {
                    Id = 0,
                    Name = "Bazylia",
                    PlantTypes = new ObservableCollection<PlantType>(plantTypes.Where(x => x.Name == "Ziele" || x.Name == "Jadalne" || x.Name == "Ozdobne" || x.Name == "Warzywo")),
                    PlantingDate = new DateTime(2023, 09, 16),
                    Containers = new ObservableCollection<PlantContainer>(containers.Where(x => x.Name == "Słoik")),
                    WateringSystems = new ObservableCollection<WateringSystem>(wateringSystems.Where(x => x.Name == "Hydroponika" || x.Name == "Ręczny")),
                    Position = plantPositions.Where(x => x.Name == "Parapet").FirstOrDefault()
                },
                new Plant()
                {
                    Id = 1,
                    Name = "Pomidor",
                    PlantTypes = new ObservableCollection<PlantType>(plantTypes.Where(x => x.Name == "Warzywo" || x.Name == "Jadalne").ToList()),
                    PlantingDate = new DateTime(2023, 09, 16),
                    Containers = new ObservableCollection<PlantContainer>(containers.Where(x => x.Name == "Doniczka")),
                    WateringSystems = new ObservableCollection<WateringSystem>(wateringSystems.Where(x => x.Name == "Ręczny")),
                    Position = plantPositions.Where(x => x.Name == "Parapet w Kuchni").FirstOrDefault()
                },
                new Plant()
                {
                    Id = 2,
                    Name = "Kaktus",
                    PlantTypes = new ObservableCollection<PlantType>(plantTypes.Where(x => x.Name == "Ozdobne").ToList()),
                    PlantingDate = new DateTime(2023, 09, 16),
                    Containers = new ObservableCollection<PlantContainer>(containers.Where(x => x.Name == "Słoik")),
                    WateringSystems = new ObservableCollection<WateringSystem>(wateringSystems.Where(x => x.Name == "Ręczny")),
                    Position = plantPositions.Where(x => x.Name == "Balkon").FirstOrDefault()
                }
            };

        private static ObservableCollection<Plant> plantsNoDetails = new ObservableCollection<Plant>
            {
                // add a few Plants
                new Plant()
                {
                    Id = 0,
                    Name = "Bazylia",
                    PlantingDate = new DateTime(2023, 09, 16)
                },
                new Plant()
                {
                    Id = 1,
                    Name = "Pomidor",
                    PlantingDate = new DateTime(2023, 09, 16)
                },
                new Plant()
                {
                    Id = 2,
                    Name = "Kaktus",
                    PlantingDate = new DateTime(2023, 09, 16)
                }
            };

        public ObservableCollection<WateringSystem> WateringSystem_GetAll()
        {
            return wateringSystems;
        }

        public void WateringSystem_InsertOne(WateringSystem model)
        {
            wateringSystems.Add(model);
        }

        public ObservableCollection<PlantContainer> PlantContainer_GetAll()
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

        public ObservableCollection<PlantType> PlantType_GetAvailableForPlant(int plantId)
        {
            Plant plant = Plants_GetById(plantId);
            ObservableCollection<PlantType> types = PlantType_GetAll();
            List<PlantType> toRemove = new();
            foreach (PlantType type in types)
            {
                if (plant.PlantTypes.Contains(type))
                {
                    toRemove.Add(type);
                }
            }
            foreach (PlantType type in toRemove)
            {
                types.Remove(type);
            }
            return types;
        }

        public Plant Plants_GetById(int plantId)
        {
            Plant? ret = Plants_GetAll().Where(x => x.Id == plantId).FirstOrDefault();
            if (ret is null)
                return new Plant();
            else
                return ret;
        }

        public ObservableCollection<Plant> Plants_GetAll()
        {
            return plantsFull;
        }

        public ObservableCollection<Plant> Plants_GetAllNoDetails()
        {
            return plantsNoDetails;
        }

        public ObservableCollection<WateringSystem> WateringSystem_GetByPlantId(int plantId)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<PlantType> PlantType_GetByPlantId(int plantId)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<WateringSystem> WateringSystem_GetAvailableForPlant(int plantId)
        {
            throw new NotImplementedException();
        }

        public void PlantContainer_InsertOne(PlantContainer model)
        {
            throw new NotImplementedException();
        }

        public List<PlantContainer> PlantContainer_GetByPlantId(int plantId)
        {
            throw new NotImplementedException();
        }

        public List<PlantContainer> PlantContainer_GetAvailableForPlant(int plantId)
        {
            throw new NotImplementedException();
        }

        public void WateringSystem_AddOneForPlant(int plantId, int wateringSystemId)
        {
            throw new NotImplementedException();
        }

        public void PlantType_AddOneForPlant(int plantId, int typeId)
        {
            throw new NotImplementedException();
        }

        public void PlantContainer_AddOneForPlant(int plantId, int containerId)
        {
            throw new NotImplementedException();
        }

        public void WateringSystem_RemoveOneForPlant(int plantId, int wateringSystemId)
        {
            throw new NotImplementedException();
        }

        public void PlantContainer_RemoveOneForPlant(int plantId, int containerId)
        {
            throw new NotImplementedException();
        }

        public void PlantType_RemoveOneForPlant(int plantId, int typeId)
        {
            throw new NotImplementedException();
        }

        ObservableCollection<PlantContainer> IDataAccess.PlantContainer_GetByPlantId(int plantId)
        {
            throw new NotImplementedException();
        }

        ObservableCollection<PlantContainer> IDataAccess.PlantContainer_GetAvailableForPlant(int plantId)
        {
            throw new NotImplementedException();
        }

        void IDataAccess.PlantPosition_InsertOne(PlantPosition model)
        {
            throw new NotImplementedException();
        }

        ObservableCollection<PlantPosition> IDataAccess.PlantPosition_GetAll()
        {
            throw new NotImplementedException();
        }

        ObservableCollection<PlantPosition> IDataAccess.PlantPosition_GetByPlantId(int plantId)
        {
            throw new NotImplementedException();
        }

        ObservableCollection<PlantPosition> IDataAccess.PlantPosition_GetAvailableForPlant(int plantId)
        {
            throw new NotImplementedException();
        }

        void IDataAccess.PlantPosition_AddOneForPlant(int plantId, int positionId)
        {
            throw new NotImplementedException();
        }

        void IDataAccess.PlantPosition_RemoveOneForPlant(int plantId, int positionId)
        {
            throw new NotImplementedException();
        }

        public void Plants_UpdatePosition(int plantId, int newPositionId)
        {
            throw new NotImplementedException();
        }
    }
}
