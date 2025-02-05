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
    internal class ManagePlantContainersViewModel: ManagePlantAttributes<PlantContainer>
    {
        private IDataAccess _dataAccess;

        public ManagePlantContainersViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantAttributes = _dataAccess.PlantContainer_GetAll();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }

        protected override ObservableCollection<Plant> DataAccess_GetAllPlantsWithAttribute(PlantContainer attribute)
        {
            return _dataAccess.PlantContainer_GetAllPlantsWithContainer(attribute);
        }

        protected override void DataAccess_DeleteAttribute(int id)
        {
            _dataAccess.PlantContainer_DeletePlantContainer(id);
        }

        protected override void DataAccess_InsertAttribute(PlantContainer attribute)
        {
            _dataAccess.PlantContainer_InsertOne(attribute);
        }

        protected override void DataAccess_UpdateAttribute(PlantContainer attribute)
        {
            _dataAccess.PlantContainer_Update(attribute);
        }
    }
}
