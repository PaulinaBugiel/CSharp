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
    class AddWateringSystemViewModel : PopupViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<WateringSystem> _wateringSystems;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newWateringSystemText = "";
        private WateringSystem _selectedWateringSystem;


        private RelayCommand _addNewWateringSystemCommand;
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
            _wateringSystems = _dataAccess.WateringSystem_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region Properties

        public ObservableCollection<WateringSystem> WateringSystems
        {
            get { return _wateringSystems; }
            set
            {
                if (_wateringSystems == value)
                    return;
                _wateringSystems = value;
                OnPropertyChanged(nameof(WateringSystems));
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

        public string NewWateringSystemText
        {
            get
            {
                return _newWateringSystemText;
            }
            set
            {
                if (_newWateringSystemText == value)
                    return;
                _newWateringSystemText = value;
                OnPropertyChanged(nameof(NewWateringSystemText));
            }
        }

        public WateringSystem SelectedWateringSystem
        {
            get { return _selectedWateringSystem; }
            set
            {
                if (_selectedWateringSystem == value)
                    return;
                _selectedWateringSystem = value;
                OnPropertyChanged(nameof(SelectedWateringSystem));
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Adds new PlantType with name written in NewWateringSystemText field
        /// </summary>
        public RelayCommand AddNewWateringSystemCommand
        {
            get
            {
                if (_addNewWateringSystemCommand == null)
                    _addNewWateringSystemCommand = new RelayCommand(o => AddNewWateringSystem(), o => CanAddNewWateringSystem());
                return _addNewWateringSystemCommand;
            }
        }
        bool CanAddNewWateringSystem()
        {
            if (NewWateringSystemText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewWateringSystem()
        {
            ObservableCollection<WateringSystem> allWateringSystems = _dataAccess.WateringSystem_GetAll();
            var typeWithTheSameName = allWateringSystems.Where(x => x.Name == NewWateringSystemText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Watering system \"{NewWateringSystemText}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WateringSystem? maxIdType = allWateringSystems.OrderByDescending(x => x.Id).First();
                int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                WateringSystem newWateringSystem = new WateringSystem { Name = NewWateringSystemText, Id = newId };
                NewWateringSystemText = "";
                _dataAccess.WateringSystem_InsertOne(newWateringSystem);
                WateringSystems = _dataAccess.WateringSystem_GetAvailableForPlant(_selectedPlant.Id);

            }
        }

        /// <summary>
        /// Cancels adding new WateringSystem - clears the NewWateringSystemText field
        /// </summary>
        public RelayCommand CancelAddingWateringSystemCommand
        {
            get
            {
                if (_cancelAddingWateringSystemCommand == null)
                    _cancelAddingWateringSystemCommand = new RelayCommand(o => CancelAddingWateringSystem(), o => CanCancelAddingWateringSystem());
                return _cancelAddingWateringSystemCommand;
            }
        }
        bool CanCancelAddingWateringSystem()
        {
            if (NewWateringSystemText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingWateringSystem()
        {
            NewWateringSystemText = "";
        }

        public RelayCommand AddSelectedWateringSystemCommand
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
            if (SelectedWateringSystem is null)
                return false;
            else
                return true;
        }
        void AddSelectedWateringSystem()
        {
            SelectedPlant.WateringSystems.Add(SelectedWateringSystem);
            // TODO Update the plant to database
            // Close the window
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
