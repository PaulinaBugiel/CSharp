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
    internal class ManagePlantTypesViewModel : ManagePlantAttributes<PlantType>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IDataAccess _dataAccess;

        public ManagePlantTypesViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantAttributes = _dataAccess.PlantType_GetAll();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override ObservableCollection<Plant> DataAccess_GetAllPlantsWithAttribute(PlantType attribute)
        {
            return _dataAccess.PlantType_GetAllPlantsWithType(attribute);
        }

        protected override void DataAccess_DeleteAttribute(int id)
        {
            _dataAccess.PlantType_DeletePlantType(id);
        }

        protected override void DataAccess_InsertAttribute(PlantType attribute)
        {
            _dataAccess.PlantType_InsertOne(attribute);
        }

        protected override void DataAccess_UpdateAttribute(PlantType attribute)
        {
            _dataAccess.PlantType_Update(attribute);
        }
    }
}
