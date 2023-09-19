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
                model.Id = p.Get<int>("@id");
            }
        }

        public List<WateringSystem> WateringSystem_GetAll()
        {
            List<WateringSystem> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<WateringSystem>("dbo.spWateringSystems_GetAll").ToList();
            }
            return ret;
        }

        public List<PlantContainer> Container_GetAll()
        {
            List<PlantContainer> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<PlantContainer>("dbo.spPlantContainers_GetAll").ToList();
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
            // TODO fill in
            throw new NotImplementedException();
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

        public ObservableCollection<PlantType> PlantType_GetAvailableForPlant(int plantId)
        {
            throw new NotImplementedException();
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
                    plant.WateringSystems = new List<WateringSystem>();
                }
            }
            return ret;
        }

        public Plant Plants_GetById(int plantId)
        {
            throw new NotImplementedException();
        }
    }
}
