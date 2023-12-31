﻿using PlantTrackerUI.Core;
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

        public ObservableCollection<WateringSystem> PlantAttributes
        {
            get { return _plantTypes; }
            set
            {
                if (_plantTypes == value)
                    return;
                _plantTypes = value;
                OnPropertyChanged(nameof(PlantAttributes));
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

        public string NewAttributeText
        {
            get
            {
                return _newPlantTypeText;
            }
            set
            {
                if (_newPlantTypeText == value)
                    return;
                _newPlantTypeText = value;
                OnPropertyChanged(nameof(NewAttributeText));
            }
        }

        public string AddSelectedAttributeButtonText { get { return "Add Selected Watering System"; } set { } }
        public string NewAttributeLabelText { get { return "New Watering System:"; } set { } }

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

        /// <summary>
        /// Adds new PlantType with name written in NewPlantTypeText field
        /// </summary>
        public RelayCommand AddNewAttributeCommand
        {
            get
            {
                if (_addNewPlantTypeCommand == null)
                    _addNewPlantTypeCommand = new RelayCommand(o => AddNewPlantType(), o => CanAddNewPlantType());
                return _addNewPlantTypeCommand;
            }
        }
        bool CanAddNewPlantType()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewPlantType()
        {
            ObservableCollection<WateringSystem> allWateringSystems = _dataAccess.WateringSystem_GetAll();
            var typeWithTheSameName = allWateringSystems.Where(x => x.Name == NewAttributeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Watering system \"{NewAttributeText}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WateringSystem? maxIdType = allWateringSystems.OrderByDescending(x => x.Id).First();
                int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                WateringSystem newWateringSystem = new WateringSystem { Name = NewAttributeText, Id = newId };
                NewAttributeText = "";
                _dataAccess.WateringSystem_InsertOne(newWateringSystem);
                PlantAttributes = _dataAccess.WateringSystem_GetAvailableForPlant(_selectedPlant.Id);

            }
        }

        /// <summary>
        /// Cancels adding new WateringSystem - clears the NewWateringSystemText field
        /// </summary>
        public RelayCommand CancelAddingAttributeCommand
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
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingWateringSystem()
        {
            NewAttributeText = "";
        }

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
