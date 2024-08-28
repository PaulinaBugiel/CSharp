using Dapper;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private const string connStringName = "PlantDbZero_String";
        private readonly string connString = System.Configuration.ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;

        public void WateringSystem_InsertOne(WateringSystem model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spWateringSystems_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
            }
        }

        public void WateringSystem_Update(WateringSystem wateringSystemToUpdate)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@WateringSystemId", wateringSystemToUpdate.Id);
                p.Add("@Name", wateringSystemToUpdate.Name);
                connection.Execute("dbo.spWateringSystems_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<WateringSystem> WateringSystem_GetAll()
        {
            ObservableCollection<WateringSystem> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetAll"));
            }
            return ret;
        }

        public ObservableCollection<WateringSystem> WateringSystem_GetByPlantId(int plantId)
        {
            ObservableCollection<WateringSystem> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetByPlantId", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }
        public ObservableCollection<WateringSystem> WateringSystem_GetAvailableForPlant(int plantId)
        {
            ObservableCollection<WateringSystem> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetGetAvailableForPlant", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }

        public void WateringSystem_AddOneForPlant(int plantId, int wateringSystemId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@WateringSystemId", wateringSystemId);
                connection.Execute("dbo.spWateringSystems_AddWateringSystemForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void WateringSystem_RemoveOneForPlant(int plantId, int wateringSystemId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@WateringSystemId", wateringSystemId);
                connection.Execute("dbo.spPlantWateringSystems_RemoveWateringSystemForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void WateringSystem_DeleteWateringSystem(int wateringSystemId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@WateringSystemId", wateringSystemId);
                connection.Execute("dbo.spWateringSystems_DeleteWateringSystem", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<Plant> WateringSystem_GetAllPlantsWithSystem(WateringSystem wateringSystem)
        {
            ObservableCollection<Plant> ret;
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@WateringSystemId", wateringSystem.Id);
                ret = new ObservableCollection<Plant>(connection.Query<Plant>("dbo.spWateringSystems_GetAllPlantsWithSystem", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }


        public void PlantContainer_InsertOne(PlantContainer model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Capacity", model.Capacity);
                p.Add("@Color", model.Color);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spPlantContainers_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
            }
        }

        public void PlantContainer_Update(PlantContainer containerToUpdate)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@ContainerId", containerToUpdate.Id);
                p.Add("@Name", containerToUpdate.Name);
                p.Add("@Capacity", containerToUpdate.Capacity);
                p.Add("@Color", containerToUpdate.Color);
                connection.Execute("dbo.spPlantContainers_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<PlantContainer> PlantContainer_GetAll()
        {
            ObservableCollection<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = new ObservableCollection<PlantContainer>(connection.Query<PlantContainer>("dbo.spPlantContainers_GetAll"));
            }
            return ret;
        }

        public ObservableCollection<PlantContainer> PlantContainer_GetByPlantId(int plantId)
        {
            ObservableCollection<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<PlantContainer>(connection.Query<PlantContainer>("dbo.spPlantContainers_GetByPlantId", p, commandType: CommandType.StoredProcedure));
            }
            return ret;

        }

        public ObservableCollection<PlantContainer> PlantContainer_GetAvailableForPlant(int plantId)
        {
            ObservableCollection<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<PlantContainer>(connection.Query<PlantContainer>("dbo.spPlantContainers_GetGetAvailableForPlant", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }

        public void PlantContainer_AddOneForPlant(int plantId, int containerId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@ContainerId", containerId);
                connection.Execute("dbo.spPlantContainers_AddContainerForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void PlantContainer_RemoveOneForPlant(int plantId, int containerId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@ContainerId", containerId);
                connection.Execute("dbo.spPlantContainers_RemoveContainerForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void PlantContainer_DeletePlantContainer(int plantContainerId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@ContainerId", plantContainerId);
                connection.Execute("dbo.spPlantContainers_DeletePlantContainer", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<Plant> PlantContainer_GetAllPlantsWithContainer(PlantContainer container)
        {
            ObservableCollection<Plant> ret;
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@ContainerId", container.Id);
                ret = new ObservableCollection<Plant>(connection.Query<Plant>("dbo.spPlantContainers_GetAllPlantsWithContainer", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }


        public void PlantPosition_InsertOne(PlantPosition model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@SunExpositionId", (int) model.Exposition);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spPlantPositions_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
            }
        }

        public void PlantPosition_Update(PlantPosition plantPositionToUpdate)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PositionId", plantPositionToUpdate.Id);
                p.Add("@Name", plantPositionToUpdate.Name);
                p.Add("@SunExpositionId", (int)plantPositionToUpdate.Exposition);
                connection.Execute("dbo.spPlantPositions_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<PlantPosition> PlantPosition_GetAll()
        {
            ObservableCollection<PlantPosition> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var queryResult = connection.Query<PlantPosition>("dbo.spPlantPositions_GetAll");
                ret = new ObservableCollection<PlantPosition>(queryResult);
            }
            return ret;
        }

        public PlantPosition? PlantPosition_GetByPlantId(int plantId)
        {
            PlantPosition? ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = connection.Query<PlantPosition>("dbo.spPlantPositions_GetByPlantId", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return ret;

        }

        public ObservableCollection<PlantPosition> PlantPosition_GetAvailableForPlant(int plantId)
        {
            ObservableCollection<PlantPosition> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<PlantPosition>(connection.Query<PlantPosition>("dbo.spPlantPositions_GetGetAvailableForPlant", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }

        public void PlantPosition_AddOneForPlant(int plantId, int positionId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@PositionId", positionId);
                connection.Execute("dbo.spPlantPositions_AddContainerForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void PlantPosition_RemoveOneForPlant(int plantId, int positionId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@PositionId", positionId);
                connection.Execute("dbo.spPlantPositions_RemovePositionForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void PlantPosition_DeletePlantPosition(int positionId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PositionId", positionId);
                connection.Execute("dbo.spPlantPosition_DeletePlantPosition", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<Plant> PlantPosition_GetAllPlantsWithPosition(PlantPosition position)
        {
            ObservableCollection<Plant> ret;
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PositionId", position.Id);
                ret = new ObservableCollection<Plant>(connection.Query<Plant>("dbo.spPlantPositions_GetAllPlantsWithPosition", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }


        public void PlantType_InsertOne(PlantType model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spPlantTypes_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
            }
        }

        public void PlantType_Update(PlantType plantTypeToUpdate)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@TypeId", plantTypeToUpdate.Id);
                p.Add("@Name", plantTypeToUpdate.Name);
                connection.Execute("dbo.spPlantTypes_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<PlantType> PlantType_GetAll()
        {
            ObservableCollection<PlantType> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = new ObservableCollection<PlantType>(connection.Query<PlantType>("dbo.spPlantTypes_GetAll"));
            }
            return ret;
        }

        public ObservableCollection<PlantType> PlantType_GetByPlantId(int plantId)
        {
            ObservableCollection<PlantType> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<PlantType>(
                    connection.Query<PlantType>("dbo.spPlantTypes_GetByPlantId", p, commandType: CommandType.StoredProcedure)
                );
            }
            return ret;
        }

        public ObservableCollection<PlantType> PlantType_GetAvailableForPlant(int plantId)
        {
            ObservableCollection<PlantType> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<PlantType>(
                    connection.Query<PlantType>("dbo.spPlantTypes_GetGetAvailableForPlant", p, commandType: CommandType.StoredProcedure)
                    );
            }
            return ret;
        }

        public void PlantType_AddOneForPlant(int plantId, int typeId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@TypeId", typeId);
                connection.Execute("dbo.spPlantTypes_AddTypeForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void PlantType_RemoveOneForPlant(int plantId, int typeId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@TypeId", typeId);
                connection.Execute("dbo.spPlantTypes_RemoveTypeForPlant", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void PlantType_DeletePlantType(int typeId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@TypeId", typeId);
                connection.Execute("dbo.spPlantTypes_DeletePlantType", p, commandType: CommandType.StoredProcedure);
            }
        }

        public ObservableCollection<Plant> PlantType_GetAllPlantsWithType(PlantType plantType)
        {
            ObservableCollection<Plant> ret;
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantTypeId", plantType.Id);
                ret = new ObservableCollection<Plant>(connection.Query<Plant>("dbo.spPlantTypes_GetAllPlantsWithType", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }


        public ObservableCollection<Plant> Plants_GetAll()
        {
            ObservableCollection<Plant> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = new ObservableCollection<Plant>(connection.Query<Plant>("dbo.spPlants_GetAll"));
                foreach (Plant plant in ret)
                {
                    var p = new DynamicParameters();
                    p.Add("@PlantId", plant.Id);
                    plant.Position = connection.Query<PlantPosition>("dbo.spPlantPositions_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure).ToList().FirstOrDefault();
                    plant.Containers = new ObservableCollection<PlantContainer>(connection.Query<PlantContainer>("dbo.spPlantContainers_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure));
                    plant.PlantTypes = new ObservableCollection<PlantType>(connection.Query<PlantType>("dbo.spPlantTypes_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure));
                    plant.WateringSystems = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure));
                }
            }
            return ret;
        }


        public ObservableCollection<Plant> Plants_GetAllNoDetails()
        {
            ObservableCollection<Plant> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = new ObservableCollection<Plant>(connection.Query<Plant>("dbo.spPlants_GetAll"));
            }
            return ret;
        }

        public Plant Plants_GetById(int plantId)
        {
            Plant? ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = connection.Query<Plant>("dbo.spPlants_GetById", p, commandType: CommandType.StoredProcedure).ToList().FirstOrDefault();
                if (ret is null)
                    return new Plant();
                ret.Position = connection.Query<PlantPosition>("dbo.spPlantPositions_GetByPlantId", p, commandType: CommandType.StoredProcedure).ToList().FirstOrDefault();
                ret.Containers = new ObservableCollection<PlantContainer>(connection.Query<PlantContainer>("dbo.spPlantContainers_GetByPlantId", p, commandType: CommandType.StoredProcedure));
                ret.PlantTypes = new ObservableCollection<PlantType>(connection.Query<PlantType>("dbo.spPlantTypes_GetByPlantId", p, commandType: CommandType.StoredProcedure));
                ret.WateringSystems = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetByPlantId", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }

        public void Plants_UpdatePosition(int plantId, int newPositionId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                p.Add("@PositionId", newPositionId);
                connection.Execute("dbo.spPlants_UpdatePosition", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
