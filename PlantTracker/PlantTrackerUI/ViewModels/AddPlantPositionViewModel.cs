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
using System.Windows.Navigation;

namespace PlantTrackerUI.ViewModels
{
    class AddPlantPositionViewModel : PopupViewModelBase, IAddPlantAttribute<PlantPosition>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<PlantPosition> _availablePlantPositions;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newPositionText = "";

        private PlantPosition? _selectedPosition;

        private RelayCommand? _addNewPositionCommand;
        private RelayCommand? _cancelAddingNewPositionCommand;
        private RelayCommand? _addSelectedPositionCommand;

        public AddPlantPositionViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();

            _selectedPlant = selectedPlant;
            _availablePlantPositions = _dataAccess.PlantPosition_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region IAddPlantAttribute
        public ObservableCollection<PlantPosition> AvailablePlantAttributes 
        {
            get { return _availablePlantPositions; }
            set
            {
                if (_availablePlantPositions == value)
                    return;
                _availablePlantPositions = value;
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


        public string AddSelectedAttributeButtonText { get { return "Add Selected Position";  } set { } }
        public PlantPosition SelectedAttribute
        {
            get { return _selectedPosition; }
            set
            {
                if (_selectedPosition == value)
                    return;
                _selectedPosition = value;
            }
        }


        public RelayCommand AddSelectedAttributeCommand
        {
            get
            {
                if (_addSelectedPositionCommand == null)
                    _addSelectedPositionCommand = new RelayCommand(o => AddSelectedPosition(), o => CanAddSelectedPosition());
                return _addSelectedPositionCommand;
            }
        }
        bool CanAddSelectedPosition()
        {
            if (SelectedAttribute is null)
                return false;
            else
                return true;
        }
        void AddSelectedPosition()
        {
            SelectedPlant.Position = SelectedAttribute;
            _dataAccess.PlantPosition_AddOneForPlant(SelectedPlant.Id, SelectedAttribute.Id); // TODO there can be only one position!
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
