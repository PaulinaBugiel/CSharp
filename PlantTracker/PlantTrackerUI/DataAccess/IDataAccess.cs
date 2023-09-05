using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.DataAccess
{
    public interface IDataAccess
    {
        void WateringSystem_InsertOne(WateringSystem model);
        List<WateringSystem> WateringSystem_GetAll();

        List<PlantContainer> Container_GetAll();

        List<PlantPosition> PlantPosition_GetAll();

        void PlantType_InsertOne(PlantType model);
        List<PlantType> PlantType_GetAll();


        List<Plant> Plants_GetAll();
    }
}
