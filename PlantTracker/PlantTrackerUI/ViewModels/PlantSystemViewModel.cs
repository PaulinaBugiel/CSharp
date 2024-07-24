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
        private RelayCommand _openAddContainerWindowCommand;
        private RelayCommand _openAddPositionWindowCommand;
        private RelayCommand _removeSelectedTypeCommand;
        private RelayCommand _removeSelectedWateringSystemCommand;
        private RelayCommand _removeSelectedContainerCommand;
        private RelayCommand _removeSelectedPositionCommand; // TODO there is only one position!

        public PlantSystemViewModel(IWindowService genericWindowService)
        {
            string? dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();

            PlantsList = _dataAccess.Plants_GetAllNoDetails();
            _windowService = genericWindowService;
            _openAddPlantTypeWindowCommand = new RelayCommand(o => OpenAddPlantTypeWindow(SelectedPlant)); // TODO remove argument?
            _removeSelectedTypeCommand = new RelayCommand((o) => RemoveSelectedType(o));

            _openAddWateringSystemWindowCommand = new RelayCommand(o => OpenAddWateringSystemWindow(SelectedPlant));
            _removeSelectedWateringSystemCommand = new RelayCommand((o) => RemoveSelectedWateringSystem(o));

            _openAddContainerWindowCommand = new RelayCommand(o => OpenAddContainerWindow(SelectedPlant));
            _removeSelectedContainerCommand = new RelayCommand((o) => RemoveSelectedContainer(o));

            _openAddPositionWindowCommand = new RelayCommand(o => OpenAddPositionWindow(SelectedPlant));
            _removeSelectedPositionCommand = new RelayCommand(o => RemoveSelectedPosition(o));
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

            PlantType toRemove = (PlantType)typeToRemove;
            SelectedPlant.PlantTypes.Remove(toRemove);
            _dataAccess.PlantType_RemoveOneForPlant(SelectedPlant.Id, toRemove.Id);

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

            WateringSystem toRemove = (WateringSystem)wateringSystemToRemove;
            SelectedPlant.WateringSystems.Remove(toRemove);
            _dataAccess.WateringSystem_RemoveOneForPlant(SelectedPlant.Id, toRemove.Id);
        }

        public RelayCommand RemoveSelectedWateringSystemCommand
        {
            get { return _removeSelectedWateringSystemCommand; }
        }


        public void OpenAddContainerWindow(Plant plant)
        {
            _windowService.ShowWindow(new AddPlantContainerViewModel(SelectedPlant));
        }
        public RelayCommand OpenAddContainerWindowCommand
        {
            get { return _openAddContainerWindowCommand; }
        }

        void RemoveSelectedContainer(object containerToRemove)
        {
            if (containerToRemove is not PlantContainer)
                return;

            PlantContainer toRemove = (PlantContainer)containerToRemove;
            SelectedPlant.Containers.Remove(toRemove);
            _dataAccess.PlantContainer_RemoveOneForPlant(SelectedPlant.Id, toRemove.Id);
        }
        public RelayCommand RemoveSelectedContainerCommand
        {
            get { return _removeSelectedContainerCommand; }
        }

        public void OpenAddPositionWindow(Plant plant)
        {
            _windowService.ShowWindow(new AddPlantPositionViewModel(SelectedPlant));
        }
        public RelayCommand OpenAddPositionWindowCommand
        {
            get { return _openAddPositionWindowCommand; }
        }

        void RemoveSelectedPosition(object positionToRemove) // TODO remove positionToRemove
        {
            if (positionToRemove is not PlantPosition)
                return;

            PlantPosition toRemove = (PlantPosition)positionToRemove;
            SelectedPlant.Position = null; // TODO there can be only one position!!
            _dataAccess.PlantContainer_RemoveOneForPlant(SelectedPlant.Id, toRemove.Id); // ?????
        }
        public RelayCommand RemoveSelectedPositionCommand
        {
            get { return _removeSelectedPositionCommand; }
        }
        #endregion

        protected virtual void OnPropertyChanged(string? propertyName = null)  // TODO Add to ViewModelBase
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
