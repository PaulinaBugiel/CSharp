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
    class ManageWateringSystemsViewModel : ManagePlantAttributes<WateringSystem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IDataAccess _dataAccess;

        public ManageWateringSystemsViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantAttributes = _dataAccess.WateringSystem_GetAll();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override ObservableCollection<Plant> DataAccess_GetAllPlantsWithAttribute(WateringSystem attribute)
        {
            return _dataAccess.WateringSystem_GetAllPlantsWithSystem(attribute);
        }

        protected override void DataAccess_DeleteAttribute(int id)
        {
            _dataAccess.WateringSystem_DeleteWateringSystem(id);
        }

        protected override void DataAccess_InsertAttribute(WateringSystem attribute)
        {
            _dataAccess.WateringSystem_InsertOne(attribute);
        }

        protected override void DataAccess_UpdateAttribute(WateringSystem attribute)
        {
            _dataAccess.WateringSystem_Update(attribute);
        }
    }
}
