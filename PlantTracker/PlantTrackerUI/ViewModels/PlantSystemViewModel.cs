using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using PlantTrackerUI.Services;
using PlantTrackerUI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.ViewModels
{
    public class PlantSystemViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public List<Plant> PlantsList { get; set; } = new List<Plant>();
        //public List<PlantType> PlantTypes { get; set; } = new List<PlantType>();
        //public List<PlantContainer> Containers { get; set; } = new List<PlantContainer>();
        //public List<WateringSystem> WateringSystems { get; set; } = new List<WateringSystem>();
        //public List<PlantPosition> Positions { get; set; } = new List<PlantPosition>();
        private readonly IDataAccess _dataAccess;

        private Plant _selectedPlant;
        private IWindowService _windowService;

        private RelayCommand _openAddPlantTypeWindowCommand;

        public PlantSystemViewModel(IWindowService windowService)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();

            PlantsList = _dataAccess.Plants_GetAll();
            _windowService = windowService;
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


        public void OpenAddPlantTypeWindow(Plant plant)
        {
            _windowService.ShowWindow(new AddPlantTypeViewModel(SelectedPlant));
        }
        public RelayCommand OpenAddPlantTypeWindowCommand
        {
            get
            {
                if (_openAddPlantTypeWindowCommand == null)
                    _openAddPlantTypeWindowCommand = new RelayCommand(o => OpenAddPlantTypeWindow(SelectedPlant));
                return _openAddPlantTypeWindowCommand;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
