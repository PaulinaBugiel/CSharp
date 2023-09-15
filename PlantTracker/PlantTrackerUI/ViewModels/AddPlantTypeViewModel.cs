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
    class AddPlantTypeViewModel : PopupViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<PlantType> _plantTypes;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newTypeText = "";
        private PlantType _selectedType;


        private RelayCommand _addNewTypeCommand;
        private RelayCommand _cancelAddingTypeCommand;
        private RelayCommand _addSelectedTypeCommand;


        public AddPlantTypeViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            _plantTypes = _dataAccess.PlantType_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region Properties

        public ObservableCollection<PlantType> PlantTypes
        {
            get { return _plantTypes; }
            set
            {
                if (_plantTypes == value)
                    return;
                _plantTypes = value;
                OnPropertyChanged(nameof(PlantTypes));
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

        public string NewTypeText
        {
            get
            {
                return _newTypeText;
            }
            set
            {
                if (_newTypeText == value)
                    return;
                _newTypeText = value;
                OnPropertyChanged(nameof(NewTypeText));
            }
        }

        public PlantType SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType == value)
                    return;
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Adds new PlantType with name written in NewTypeText field
        /// </summary>
        public RelayCommand AddNewTypeCommand
        {
            get
            {
                if (_addNewTypeCommand == null)
                    _addNewTypeCommand = new RelayCommand(o => AddNewType(), o => CanAddNewType());
                return _addNewTypeCommand;
            }
        }
        bool CanAddNewType()
        {
            if (NewTypeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewType()
        {
            ObservableCollection<PlantType> allTypes = _dataAccess.PlantType_GetAll();
            var typeWithTheSameName = allTypes.Where(x => x.Name == NewTypeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Plant type \"{NewTypeText}\" already exists", "Cannot add plant type",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlantType? maxIdType = allTypes.OrderByDescending(x => x.Id).First();
                int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                PlantType newType = new PlantType { Name = NewTypeText, Id = newId };
                NewTypeText = "";
                _dataAccess.PlantType_InsertOne(newType);
                PlantTypes = _dataAccess.PlantType_GetAvailableForPlant(_selectedPlant.Id);

            }
        }

        /// <summary>
        /// Cancels adding new PlantType - clears the NewTypeText field
        /// </summary>
        public RelayCommand CancelAddingTypeCommand
        {
            get
            {
                if (_cancelAddingTypeCommand == null)
                    _cancelAddingTypeCommand = new RelayCommand(o => CancelAddingType(), o => CanCancelAddingType());
                return _cancelAddingTypeCommand;
            }
        }
        bool CanCancelAddingType()
        {
            if (NewTypeText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingType()
        {
            NewTypeText = "";
        }

        public RelayCommand AddSelectedTypeCommand
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
            if (SelectedType is null)
                return false;
            else
                return true;
        }
        void AddSelectedType()
        {
            SelectedPlant.PlantTypes.Add(SelectedType);
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
