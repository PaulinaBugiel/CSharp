using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.DataAccess
{
    public interface IDataAccess
    {
        void WateringSystem_InsertOne(WateringSystem model);
        ObservableCollection<WateringSystem> WateringSystem_GetAll();
        ObservableCollection<WateringSystem> WateringSystem_GetByPlantId(int plantId);
        ObservableCollection<WateringSystem> WateringSystem_GetAvailableForPlant(int plantId);
        void WateringSystem_AddOneForPlant(int plantId, int wateringSystemId);
        void WateringSystem_RemoveOneForPlant(int plantId, int wateringSystemId);

        void PlantContainer_InsertOne(PlantContainer model);
        List<PlantContainer> PlantContainer_GetAll();
        List<PlantContainer> PlantContainer_GetByPlantId(int plantId);
        List<PlantContainer> PlantContainer_GetAvailableForPlant(int plantId);
        void PlantContainer_AddOneForPlant(int plantId, int containerId);
        void PlantContainer_RemoveOneForPlant(int plantId, int containerId);

        List<PlantPosition> PlantPosition_GetAll();

        void PlantType_InsertOne(PlantType model);
        ObservableCollection<PlantType> PlantType_GetAll();
        ObservableCollection<PlantType> PlantType_GetByPlantId(int plantId);
        ObservableCollection<PlantType> PlantType_GetAvailableForPlant(int plantId);
        void PlantType_AddOneForPlant(int plantId, int typeId);
        void PlantType_RemoveOneForPlant(int plantId, int typeId);


        List<Plant> Plants_GetAll();
        List<Plant> Plants_GetAllNoDetails();
        Plant Plants_GetById(int plantId);
    }
}
