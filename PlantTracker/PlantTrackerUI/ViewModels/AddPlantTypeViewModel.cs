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
    class AddPlantTypeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<PlantType> _plantTypes;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newTypeText = "";
        private RelayCommand _addNewTypeCommand;
        private RelayCommand _cancelAddingTypeCommand;


        bool CanAddNewType()
        {
            if (NewTypeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewType()
        {
            var typeWithTheSameName = PlantTypes.Where(x => x.Name == NewTypeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Plant type \"{NewTypeText}\" already exists", "Cannot add plant type",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlantType? maxIdType = PlantTypes.OrderByDescending(x => x.Id).First();
                int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                PlantType newType = new PlantType { Name = NewTypeText, Id = newId };
                NewTypeText = "";
                _dataAccess.PlantType_InsertOne(newType);
                PlantTypes = _dataAccess.PlantType_GetAll();
            }
        }
        public RelayCommand AddNewTypeCommand
        {
            get
            {
                if (_addNewTypeCommand == null)
                    _addNewTypeCommand = new RelayCommand(o => AddNewType(), o => CanAddNewType());
                return _addNewTypeCommand;
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
        public RelayCommand CancelAddingTypeCommand
        {
            get
            {
                if (_cancelAddingTypeCommand == null)
                    _cancelAddingTypeCommand = new RelayCommand(o => CancelAddingType(), o => CanCancelAddingType());
                return _cancelAddingTypeCommand;
            }
        }


        public AddPlantTypeViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            _plantTypes = _dataAccess.PlantType_GetAll();
        }

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

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
