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
        void WateringSystem_AddOneForPlant(int plantId, int wateringSystemId); // TODO: int wateringSystemId => WateringSystem wateringSystem
        void WateringSystem_RemoveOneForPlant(int plantId, int wateringSystemId); // TODO: int wateringSystemId => WateringSystem wateringSystem
        void WateringSystem_DeleteWateringSystem(int wateringSystemId);
        ObservableCollection<Plant> WateringSystem_GetAllPlantsWithSystem(WateringSystem wateringSystem);

        void PlantContainer_InsertOne(PlantContainer model);
        void PlantContainer_Update(PlantContainer containerToUpdate);
        ObservableCollection<PlantContainer> PlantContainer_GetAll();
        ObservableCollection<PlantContainer> PlantContainer_GetByPlantId(int plantId);
        ObservableCollection<PlantContainer> PlantContainer_GetAvailableForPlant(int plantId);
        void PlantContainer_AddOneForPlant(int plantId, int containerId); // TODO: int containerId => PlantContainer container
        void PlantContainer_RemoveOneForPlant(int plantId, int containerId); // TODO: int containerId => PlantContainer container
        void PlantContainer_DeletePlantContainer(int plantContainerId);
        ObservableCollection<Plant> PlantContainer_GetAllPlantsWithContainer(PlantContainer container);

        void PlantPosition_InsertOne(PlantPosition model);
        ObservableCollection<PlantPosition> PlantPosition_GetAll();
        PlantPosition? PlantPosition_GetByPlantId(int plantId);
        ObservableCollection<PlantPosition> PlantPosition_GetAvailableForPlant(int plantId);
        void PlantPosition_AddOneForPlant(int plantId, int positionId); // TODO: int positionId => PlantPosition container
        void PlantPosition_RemoveOneForPlant(int plantId, int positionId); // TODO: int positionId => PlantPosition container
        void PlantPosition_DeletePlantPosition(int positionId);
        ObservableCollection<Plant> PlantPosition_GetAllPlantsWithPosition(PlantPosition position);

        void PlantType_InsertOne(PlantType model);
        ObservableCollection<PlantType> PlantType_GetAll();
        ObservableCollection<PlantType> PlantType_GetByPlantId(int plantId);
        ObservableCollection<PlantType> PlantType_GetAvailableForPlant(int plantId);
        void PlantType_AddOneForPlant(int plantId, int typeId); // TODO: int typeId => PlantType type
        void PlantType_RemoveOneForPlant(int plantId, int typeId); // TODO: int typeId => PlantType type
        void PlantType_DeletePlantType(int typeId);
        ObservableCollection<Plant> PlantType_GetAllPlantsWithType(PlantType plantType);


        ObservableCollection<Plant> Plants_GetAll();
        ObservableCollection<Plant> Plants_GetAllNoDetails();
        Plant Plants_GetById(int plantId);
        void Plants_UpdatePosition(int plantId, int newPositionId);
    }
}
