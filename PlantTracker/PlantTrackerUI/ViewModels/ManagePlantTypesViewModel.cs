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
    internal class ManagePlantTypesViewModel : PopupViewModelBase, IManagePlantAttributes<PlantType>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<PlantType> _plantTypesToView;
        private readonly ObservableCollection<PlantType> _plantTypesInDb;
        private ObservableCollection<PlantType> _tmpAdds;
        private ObservableCollection<PlantType> _tmpDeletes;
        private IDataAccess _dataAccess;
        string _newPlantTypeText = "";

        private RelayCommand _addNewPlantTypeCommand;
        private RelayCommand _removePlantTypeCommand;
        private RelayCommand _cancelAddingPlantTypeCommand;
        private RelayCommand _applyChangesCommand;
        private RelayCommand _cancelChangesCommand;
        public ManagePlantTypesViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantTypesToView = _dataAccess.PlantType_GetAll();
            _plantTypesInDb = _dataAccess.PlantType_GetAll();
            _tmpAdds = new ObservableCollection<PlantType>();
            _tmpDeletes = new ObservableCollection<PlantType>();
        }


        public ObservableCollection<PlantType> PlantAttributes
        {
            get { return _plantTypesToView; }
            set
            {
                if (_plantTypesToView == value)
                    return;
                _plantTypesToView = value;
                OnPropertyChanged(nameof(PlantAttributes));
            }
        }

        public string NewAttributePlaceholderText { get { return "New Watering System:"; } set { } }

        public string NewAttributeText
        {
            get { return _newPlantTypeText; }
            set
            {
                if (_newPlantTypeText == value)
                    return;
                _newPlantTypeText = value;
                OnPropertyChanged(nameof(NewAttributeText));
            }
        }

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
            var typeWithTheSameName = _plantTypesToView.Where(x => x.Name == NewAttributeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Watering system \"{NewAttributeText}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlantType plantTypeToAdd = new PlantType { Name = NewAttributeText };
                _plantTypesToView.Add(plantTypeToAdd);
                _tmpAdds.Add(plantTypeToAdd);
                //PlantType? maxIdType = _plantTypesToView.OrderByDescending(x => x.Id).First();
                //int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                //PlantType newPlantType = new PlantType { Name = NewAttributeText, Id = newId };
                NewAttributeText = "";
                //_dataAccess.PlantType_InsertOne(newPlantType);
                //PlantAttributes = _dataAccess.PlantType_GetAll();
                // // OnPropertyChanged(nameof(PlantAttributes));

            }
        }


        /// <summary>
        /// Cancels adding new PlantType - clears the NewPlantTypeText field
        /// </summary>
        public RelayCommand CancelAddingAttributeCommand
        {
            get
            {
                if (_cancelAddingPlantTypeCommand == null)
                    _cancelAddingPlantTypeCommand = new RelayCommand(o => CancelAddingPlantType(), o => CanCancelAddingPlantType());
                return _cancelAddingPlantTypeCommand;
            }
        }
        bool CanCancelAddingPlantType()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingPlantType()
        {
            NewAttributeText = "";
        }

        public RelayCommand RemoveAttributeCommand
        {
            get
            {
                if (_removePlantTypeCommand == null)
                    _removePlantTypeCommand = new RelayCommand(o => RemovePlantType(o), o => CanRemovePlantType());
                return _removePlantTypeCommand;
            }
        }

        bool CanRemovePlantType()
        {
            return true;
        }
        void RemovePlantType(object toRemove)
        {
            if (toRemove is not PlantType)
                return;
            PlantType plantTypeToRemove = (PlantType)toRemove;
            // TODO PlantType_GetAllPlantsWithType
             var plantsThatUseTheSystem = _dataAccess.PlantType_GetAllPlantsWithType(plantTypeToRemove);
            if (plantsThatUseTheSystem.Count() > 0)
            {
                string plantsInUseString = string.Join("\n- ", plantsThatUseTheSystem.Select(x => x.Name));
                MessageBoxResult result = MessageBox.Show($"The following plants have this plant type:\n- {plantsInUseString}\nProceed?", "Plant type in use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _tmpDeletes.Add(plantTypeToRemove);
                    _plantTypesToView.Remove(plantTypeToRemove);
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantTypeToRemove))
                    _tmpDeletes.Add(plantTypeToRemove);
                _plantTypesToView.Remove(plantTypeToRemove);
            }
        }

        public RelayCommand ApplyChangesCommand
        {
            get
            {
                if (_applyChangesCommand == null)
                    _applyChangesCommand = new RelayCommand(o => ApplyChanges(), o => CanApplyChanges());
                return _applyChangesCommand;
            }
        }
        bool CanApplyChanges()
        {
            if (_tmpAdds.Count() == 0 && _tmpDeletes.Count() == 0)
                return false;
            return true;
        }
        void ApplyChanges()
        {
            foreach (var ws in _tmpDeletes)
            {
                _dataAccess.PlantType_DeletePlantType(ws.Id);
            }
            foreach (var ws in _tmpAdds)
            {
                _dataAccess.PlantType_InsertOne(ws);
            }
            CloseWindow();
        }

        public RelayCommand CancelChangesCommand
        {
            get
            {
                if (_cancelChangesCommand == null)
                    _cancelChangesCommand = new RelayCommand(o => CancelChanges());
                return _cancelChangesCommand;
            }
        }

        void CancelChanges()
        {
            CloseWindow();
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
