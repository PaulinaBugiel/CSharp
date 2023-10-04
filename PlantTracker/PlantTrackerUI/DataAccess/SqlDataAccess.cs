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
        public List<PlantContainer> PlantContainer_GetAll()
        {
            List<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<PlantContainer>("dbo.spPlantContainers_GetAll").ToList();
            }
            return ret;
        }
        public List<PlantContainer> PlantContainer_GetByPlantId(int plantId)
        {
            List<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = connection.Query<PlantContainer>("dbo.spPlantContainers_GetByPlantId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return ret;

        }
        public List<PlantContainer> PlantContainer_GetAvailableForPlant(int plantId)
        {
            List<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@PlantId", plantId);
                ret = connection.Query<PlantContainer>("dbo.spPlantContainers_GetGetAvailableForPlant", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return ret;
        }

        public List<PlantPosition> PlantPosition_GetAll()
        {
            List<PlantPosition> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<PlantPosition>("dbo.spPlantPositions_GetAll").ToList();
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


        public List<Plant> Plants_GetAll()
        {
            List<Plant> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<Plant>("dbo.spPlants_GetAll").ToList();
                foreach (Plant plant in ret)
                {
                    var p = new DynamicParameters();
                    p.Add("@PlantId", plant.Id);
                    plant.Position = connection.Query<PlantPosition>("dbo.spPlantPositions_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure).ToList().FirstOrDefault();
                    plant.Containers = connection.Query<PlantContainer>("dbo.spPlantContainers_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure).ToList();
                    plant.PlantTypes = new ObservableCollection<PlantType>(connection.Query<PlantType>("dbo.spPlantTypes_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure));
                    plant.WateringSystems = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetByPlantId",
                                       p, commandType: CommandType.StoredProcedure));
                }
            }
            return ret;
        }


        public List<Plant> Plants_GetAllNoDetails()
        {
            List<Plant> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<Plant>("dbo.spPlants_GetAll").ToList();
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
                ret.Containers = connection.Query<PlantContainer>("dbo.spPlantContainers_GetByPlantId", p, commandType: CommandType.StoredProcedure).ToList();
                ret.PlantTypes = new ObservableCollection<PlantType>(connection.Query<PlantType>("dbo.spPlantTypes_GetByPlantId", p, commandType: CommandType.StoredProcedure));
                ret.WateringSystems = new ObservableCollection<WateringSystem>(connection.Query<WateringSystem>("dbo.spWateringSystems_GetByPlantId", p, commandType: CommandType.StoredProcedure));
            }
            return ret;
        }

    }
}
