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
    class AddWateringSystemViewModel : PopupViewModelBase, IAddPlantAttribute<WateringSystem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<WateringSystem> _plantTypes;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newPlantTypeText = "";
        private WateringSystem _selectedPlantType;


        private RelayCommand _addNewPlantTypeCommand;
        private RelayCommand _cancelAddingWateringSystemCommand;
        private RelayCommand _addSelectedWateringSystemCommand;


        public AddWateringSystemViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            _plantTypes = _dataAccess.WateringSystem_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region Properties

        public ObservableCollection<WateringSystem> AvailablePlantAttributes
        {
            get { return _plantTypes; }
            set
            {
                if (_plantTypes == value)
                    return;
                _plantTypes = value;
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


        public string AddSelectedAttributeButtonText { get { return "Add Selected Watering System"; } set { } }

        public WateringSystem SelectedAttribute
        {
            get { return _selectedPlantType; }
            set
            {
                if (_selectedPlantType == value)
                    return;
                _selectedPlantType = value;
                OnPropertyChanged(nameof(SelectedAttribute));
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddSelectedAttributeCommand
        {
            get
            {
                if (_addSelectedWateringSystemCommand == null)
                    _addSelectedWateringSystemCommand = new RelayCommand(o => AddSelectedWateringSystem(), o => CanAddSelectedWateringSystem());
                return _addSelectedWateringSystemCommand;
            }
        }
        bool CanAddSelectedWateringSystem()
        {
            if (SelectedAttribute is null)
                return false;
            else
                return true;
        }
        void AddSelectedWateringSystem()
        {
            SelectedPlant.WateringSystems.Add(SelectedAttribute);
            _dataAccess.WateringSystem_AddOneForPlant(SelectedPlant.Id, SelectedAttribute.Id);
            OnPropertyChanged(nameof(SelectedPlant));
            CloseWindow();

        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
