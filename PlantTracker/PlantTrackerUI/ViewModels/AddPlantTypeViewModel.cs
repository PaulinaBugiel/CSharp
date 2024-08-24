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
    class AddPlantTypeViewModel : PopupViewModelBase, IAddPlantAttribute<PlantType>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<PlantType> _plantTypes;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newTypeText = "";

        private PlantType? _selectedType;


        private RelayCommand? _addNewTypeCommand;
        private RelayCommand? _cancelAddingTypeCommand;
        private RelayCommand? _addSelectedTypeCommand;


        public AddPlantTypeViewModel(Plant selectedPlant)
        {
            string? dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            _plantTypes = _dataAccess.PlantType_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region Properties

        public ObservableCollection<PlantType> AvailablePlantAttributes
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

        public string AddSelectedAttributeButtonText { get { return "Add Selected Type"; } set { } }

        public PlantType SelectedAttribute
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType == value)
                    return;
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedAttribute));
            }
        }
        #endregion

        #region Commands

        public RelayCommand AddSelectedAttributeCommand
        {
            get
            {
                if (_addSelectedTypeCommand == null)
                    _addSelectedTypeCommand = new RelayCommand(o => AddSelectedType(), o => CanAddSelectedType());
                return _addSelectedTypeCommand;
            }
        }
        bool CanAddSelectedType()
        {
            if (SelectedAttribute is null)
                return false;
            else
                return true;
        }
        void AddSelectedType()
        {
            SelectedPlant.PlantTypes.Add(SelectedAttribute);
            _dataAccess.PlantType_AddOneForPlant(SelectedPlant.Id, SelectedAttribute.Id);
            OnPropertyChanged(nameof(SelectedPlant));
            CloseWindow();
            
        }
        #endregion

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
