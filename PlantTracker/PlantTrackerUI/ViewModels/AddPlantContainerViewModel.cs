using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
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
    class AddPlantContainerViewModel : PopupViewModelBase, IAddPlantAttribute<PlantContainer>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<PlantContainer> _availablePlantContainers;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newContainerText = "";

        private PlantContainer? _selectedContainer;


        private RelayCommand? _addNewContainerCommand;
        private RelayCommand? _cancelAddingContainerCommand;
        private RelayCommand? _addSelectedContainerCommand;
        private RelayCommand? _removeAttributeCommand;


        public AddPlantContainerViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            _availablePlantContainers = _dataAccess.PlantContainer_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region Properties

        public ObservableCollection<PlantContainer> AvailablePlantAttributes
        {
            get { return _availablePlantContainers; }
            set
            {
                if (_availablePlantContainers == value)
                    return;
                _availablePlantContainers = value;
                OnPropertyChanged(nameof(AvailablePlantAttributes));
            }
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

        public string AddSelectedAttributeButtonText { get { return "Add Selected Container"; } set { } }

        public PlantContainer SelectedAttribute
        {
            get { return _selectedContainer; }
            set
            {
                if (_selectedContainer == value)
                    return;
                _selectedContainer = value;
                OnPropertyChanged(nameof(SelectedAttribute));
            }
        }
        #endregion

        #region Commands

        public RelayCommand AddSelectedAttributeCommand
        {
            get
            {
                if (_addSelectedContainerCommand == null)
                    _addSelectedContainerCommand = new RelayCommand(o => AddSelectedContainer(), o => CanAddSelectedContainer());
                return _addSelectedContainerCommand;
            }
        }
        bool CanAddSelectedContainer()
        {
            if (SelectedAttribute is null)
                return false;
            else
                return true;
        }
        void AddSelectedContainer()
        {
            SelectedPlant.Containers.Add(SelectedAttribute);
            _dataAccess.PlantContainer_AddOneForPlant(SelectedPlant.Id, SelectedAttribute.Id);
            OnPropertyChanged(nameof(SelectedPlant));
            CloseWindow();
            
        }

        public RelayCommand RemoveAttributeCommand
        {
            get
            {
                if (_removeAttributeCommand == null)
                    _removeAttributeCommand = new RelayCommand(o => RemoveContainer(), o => CanRemoveContainer());
                return _removeAttributeCommand;
            }
        }

        bool CanRemoveContainer()
        {
            return true;
        }

        void RemoveContainer()
        {

        }
        #endregion

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
