using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.ViewModels
{
    internal class ManagePlantPositionsViewModel: ManagePlantAttributes<PlantPosition>
    {
        private IDataAccess _dataAccess;

        public ManagePlantPositionsViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantAttributes = _dataAccess.PlantPosition_GetAll();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }

        protected override ObservableCollection<Plant> DataAccess_GetAllPlantsWithAttribute(PlantPosition attribute)
        {
            return _dataAccess.PlantPosition_GetAllPlantsWithPosition(attribute);
        }

        protected override void DataAccess_DeleteAttribute(int id)
        {
            _dataAccess.PlantPosition_DeletePlantPosition(id);
        }

        protected override void DataAccess_InsertAttribute(PlantPosition attribute)
        {
            _dataAccess.PlantPosition_InsertOne(attribute);
        }

        protected override void DataAccess_UpdateAttribute(PlantPosition attribute)
        {
            _dataAccess.PlantPosition_Update(attribute);
        }
    }
}
