using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.ViewModels
{
    class AddPlantTypeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public List<PlantType> PlantTypes { get; set; } = new List<PlantType>();
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;

        public AddPlantTypeViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            PlantTypes = _dataAccess.PlantType_GetAll();
        }

        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set
            {
                if (_selectedPlant == value)
                    return;
                _selectedPlant = value;
                OnPropertyChanged(nameof(SelectedPlant));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
