﻿using Dapper;
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


        public void PlantContainer_InsertOne(PlantContainer model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Capacity", model.Capacity);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spPlantContainers_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
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


        public void PlantPosition_InsertOne(PlantPosition model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@SunExpositionId", (int) model.Exposition);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spPlantContainers_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
            }
        }

        public ObservableCollection<PlantPosition> PlantPosition_GetAll()
        {
            ObservableCollection<PlantPosition> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = new ObservableCollection<PlantPosition>(connection.Query<PlantPosition>("dbo.spPlantPositions_GetAll"));
            }
            return ret;
        }

        public ObservableCollection<PlantPosition> PlantPosition_GetByPlantId(int plantId)
        {
            ObservableCollection<PlantPosition> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = new ObservableCollection<PlantPosition>(connection.Query<PlantPosition>("dbo.spPlantPositions_GetByPlantId", p, commandType: CommandType.StoredProcedure));
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


        public void PlantType_InsertOne(PlantType model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spPlantTypes_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
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
