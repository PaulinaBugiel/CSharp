using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using PlantTrackerUI.Services;
using PlantTrackerUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.ViewModels
{
    public class PlantSystemViewModel : INotifyPropertyChanged // TODO Add to ViewModelBase
    {

        public event PropertyChangedEventHandler? PropertyChanged; // TODO Add to ViewModelBase
        public List<Plant> PlantsList { get; set; } = new List<Plant>();

        private readonly IDataAccess _dataAccess;
        private IWindowService _windowService;
        private Plant? _selectedPlant;

        private RelayCommand _openAddPlantTypeWindowCommand;
        private RelayCommand _openAddWateringSystemWindowCommand;
        private RelayCommand _removeSelectedTypeCommand;
        private RelayCommand _removeSelectedWateringSystemCommand;

        public PlantSystemViewModel(IWindowService genericWindowService)
        {
            string? dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();

            PlantsList = _dataAccess.Plants_GetAllNoDetails();
            _windowService = genericWindowService;
            _openAddPlantTypeWindowCommand = new RelayCommand(o => OpenAddPlantTypeWindow(SelectedPlant));
            _removeSelectedTypeCommand = new RelayCommand((o) => RemoveSelectedType(o));
            _openAddWateringSystemWindowCommand = new RelayCommand(o => OpenAddWateringSystemWindow(SelectedPlant));
            _removeSelectedWateringSystemCommand = new RelayCommand((o) => RemoveSelectedWateringSystem(o));
        }


        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set
            {
                if (_selectedPlant == value)
                    return;
                // get plant with all details from db
                _selectedPlant = _dataAccess.Plants_GetById(value.Id);
                OnPropertyChanged(nameof(SelectedPlant));
            }
        }

        #region Commands
        public void OpenAddPlantTypeWindow(Plant plant)
        {
            _windowService.ShowWindow(new AddPlantTypeViewModel(SelectedPlant));
        }
        public RelayCommand OpenAddPlantTypeWindowCommand
        {
            get { return _openAddPlantTypeWindowCommand; }
        }


        void RemoveSelectedType(object typeToRemove)
        {
            if (typeToRemove is not PlantType)
                return;

            SelectedPlant.PlantTypes.Remove((PlantType)typeToRemove);
            // TODO Update in database

        }
        public RelayCommand RemoveSelectedTypeCommand
        {
            get { return _removeSelectedTypeCommand; }
        }


        public void OpenAddWateringSystemWindow(Plant plant)
        {
            _windowService.ShowWindow(new AddWateringSystemViewModel(SelectedPlant));
        }
        public RelayCommand OpenAddWateringSystemWindowCommand
        {
            get { return _openAddWateringSystemWindowCommand; }
        }


        void RemoveSelectedWateringSystem(object wateringSystemToRemove)
        {
            if (wateringSystemToRemove is not WateringSystem)
                return;

            SelectedPlant.WateringSystems.Remove((WateringSystem)wateringSystemToRemove);
            // TODO Update in database

        }
        public RelayCommand RemoveSelectedWateringSystemCommand
        {
            get { return _removeSelectedWateringSystemCommand; }
        }
        #endregion

        protected virtual void OnPropertyChanged(string? propertyName = null)  // TODO Add to ViewModelBase
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
