using Dapper;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private const string connStringName = "PlandDbZero_String";
        private readonly string connString = System.Configuration.ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;

        public void WateringSystem_InsertOne(WateringSystem model)
        {
            using (IDbConnection connection = new SqlConnection(connString))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                connection.Execute("dbo.spWateringSystem_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public List<WateringSystem> WateringSystem_GetAll()
        {
            List<WateringSystem> ret;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                ret = connection.Query<WateringSystem>("dbo.spWateringSystem_GetAll").ToList();
            }
            return ret;
        }

        public List<Plant> Plants_GetAll()
        {
            // TODO fill in
            return new List<Plant>();
        }

        public List<PlantContainer> Container_GetAll()
        {
            // TODO fill in
            throw new NotImplementedException();
        }

        public List<PlantPosition> PlantPosition_GetAll()
        {
            // TODO fill in
            throw new NotImplementedException();
        }

        public List<PlantType> PlantType_GetAll()
        {
            // TODO fill in
            throw new NotImplementedException();
        }
    }
}
