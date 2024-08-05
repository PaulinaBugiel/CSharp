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
        public ObservableCollection<Plant> AllPlantsNoDetails { get; set; } = new ObservableCollection<Plant>();
        public ObservableCollection<Plant> AllPlantsWithDetails { get; set; } = new ObservableCollection<Plant>();
        public ObservableCollection<PlantPosition> PlantPositions { get; set; } = new ObservableCollection<PlantPosition>();

        private readonly IDataAccess _dataAccess;
        private IWindowService _windowService;
        private Plant? _selectedPlant;
        private PlantPosition? _currentPlantPosition;

        private RelayCommand _openAddPlantTypeWindowCommand;
        private RelayCommand _removeSelectedTypeCommand;

        private RelayCommand _openAddWateringSystemWindowCommand;
        private RelayCommand _removeSelectedWateringSystemCommand;
        private RelayCommand _openManageWateringSystemsWindowCommand;

        private RelayCommand _openAddContainerWindowCommand;
        private RelayCommand _removeSelectedContainerCommand;

        private RelayCommand _openAddPositionWindowCommand;

        public PlantSystemViewModel(IWindowService genericWindowService)
        {
            string? dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();

            AllPlantsNoDetails = _dataAccess.Plants_GetAllNoDetails();
            PlantPositions = _dataAccess.PlantPosition_GetAll();
            _windowService = genericWindowService;

            _openAddPlantTypeWindowCommand = new RelayCommand(o => OpenAddPlantTypeWindow(SelectedPlant)); // TODO remove argument?
            _removeSelectedTypeCommand = new RelayCommand(o => RemoveSelectedType(o));

            _openAddWateringSystemWindowCommand = new RelayCommand(o => OpenAddWateringSystemWindow(SelectedPlant));
            _removeSelectedWateringSystemCommand = new RelayCommand(o => RemoveSelectedWateringSystem(o));
            _openManageWateringSystemsWindowCommand = new RelayCommand(o => OpenManageWateringSystemsWindow());


            _openAddContainerWindowCommand = new RelayCommand(o => OpenAddContainerWindow(SelectedPlant));
            _removeSelectedContainerCommand = new RelayCommand(o => RemoveSelectedContainer(o));

            _openAddPositionWindowCommand = new RelayCommand(o => OpenAddPositionWindow(SelectedPlant));


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
                _currentPlantPosition = PlantPositions.FirstOrDefault(x => x.Name == SelectedPlant?.Position?.Name);
                OnPropertyChanged(nameof(CurrentPlantPosition));
                OnPropertyChanged(nameof(SelectedPlant));
            }
        }

        public PlantPosition CurrentPlantPosition
        {
            get { return _currentPlantPosition; }
            set
            {
                if (_currentPlantPosition == value)
                    return;
                _currentPlantPosition = value;
                _dataAccess.Plants_UpdatePosition(SelectedPlant.Id, _currentPlantPosition.Id);
                OnPropertyChanged(nameof(CurrentPlantPosition));
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


        //  TODO OpenManageWateringSystemsWindow(Plant plant)

        public void OpenAddWateringSystemWindow(Plant plant)
        {
            _windowService.ShowWindow(new AddWateringSystemViewModel(SelectedPlant));
        }
        public RelayCommand OpenAddWateringSystemWindowCommand
        {
            get { return _openAddWateringSystemWindowCommand; }
        }

        public void OpenManageWateringSystemsWindow()
        {
            _windowService.ShowWindow(new ManageWateringSystemsViewModel());
        }
        public RelayCommand OpenManageWateringSystemsWindowCommand
        {
            get { return _openManageWateringSystemsWindowCommand; }
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

        #endregion

        protected virtual void OnPropertyChanged(string? propertyName = null)  // TODO Add to ViewModelBase
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
