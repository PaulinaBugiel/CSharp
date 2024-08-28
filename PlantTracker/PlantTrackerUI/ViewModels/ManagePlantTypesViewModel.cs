using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private PlantType _selectedPlantType;
        private ObservableCollection<PlantType> _tmpUpdates;
        private ObservableCollection<PlantType> _tmpAdds;
        private ObservableCollection<PlantType> _tmpDeletes;
        private IDataAccess _dataAccess;
        string _newPlantTypeText = "";

        private RelayCommand _addNewRowCommand;
        private RelayCommand _removePlantTypeCommand;
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
            _tmpAdds = new();
            _tmpDeletes = new();
            _tmpUpdates = new();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
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

        public PlantType SelectedAttribute
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


        /// <summary>
        /// Fired when an element is added to DataGrid or deleted via 'Delete' button (either of these actions has to be enabled in DataGrid)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains OldItems - deleted items, and NewItems - newly added items</param>
        public void OnPlantAttributesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null && e.NewItems.Count > 0)
            {
                ObservableCollection<PlantType> newItems = new();
                foreach (PlantType item in e.NewItems)
                {
                    newItems.Add(item);
                    _tmpAdds.Add(item);
                }
                SubscribeCollectionItems(newItems);

            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                ObservableCollection<PlantType> oldItems = new();
                foreach (var item in e.OldItems)
                    oldItems.Add(item as PlantType);
                UnsubscribeCollectionItems(oldItems);
            }
        }

        private void SubscribeCollectionItems(ObservableCollection<PlantType> items)
        {
            if (items != null)
            {
                foreach (var item in items) item.PropertyChanged += ItemChanged;
            }
        }

        private void UnsubscribeCollectionItems(ObservableCollection<PlantType> items)
        {
            if (items != null)
            {
                foreach (var item in items) item.PropertyChanged -= ItemChanged;
            }
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender != null)
            {
                PlantType? container = sender as PlantType;
                if (e.PropertyName == "Name")
                {
                    var containerWithTheSameName = _plantTypesToView.FirstOrDefault(x => x.Name == container.Name);
                    if (containerWithTheSameName is not null && containerWithTheSameName != container)
                    {
                        MessageBox.Show($"Plant container \"{container.Name}\" already exists", "Name duplicate",
                            MessageBoxButton.OK);
                    }
                }
                // If the attribute is already in newly added or updated attributes, there's no need to add it again
                if (!(_tmpUpdates.Contains(container) || _tmpAdds.Contains(container)))
                    _tmpUpdates.Add(container);
            }
        }


        public RelayCommand AddNewRowCommand
        {
            get
            {
                if (_addNewRowCommand == null)
                    _addNewRowCommand = new RelayCommand(o => AddNewRow());
                return _addNewRowCommand;
            }
        }

        void AddNewRow()
        {
            PlantAttributes.Add(new());
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
            var plantsThatUseTheSystem = _dataAccess.PlantType_GetAllPlantsWithType(plantTypeToRemove);
            if (plantsThatUseTheSystem.Count() > 0)
            {
                string plantsInUseString = string.Join("\n- ", plantsThatUseTheSystem.Select(x => x.Name));
                MessageBoxResult result = MessageBox.Show($"The following plants have this plant type:\n- {plantsInUseString}\nProceed?", "Plant type in use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _tmpDeletes.Add(plantTypeToRemove);
                    _plantTypesToView.Remove(plantTypeToRemove);
                    _tmpUpdates.Remove(plantTypeToRemove);
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantTypeToRemove))
                    _tmpDeletes.Add(plantTypeToRemove);
                _plantTypesToView.Remove(plantTypeToRemove);
                _tmpUpdates.Remove(plantTypeToRemove);
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
            if (_tmpUpdates.Count() > 0 || _tmpAdds.Count() > 0 || _tmpDeletes.Count() > 0)
                return true;
            return false;
        }
        void ApplyChanges()
        {
            foreach (var pt in _tmpDeletes)
                _dataAccess.PlantType_DeletePlantType(pt.Id);
            foreach (var pt in _tmpAdds)
                _dataAccess.PlantType_InsertOne(pt);
            foreach (var pt in _tmpUpdates)
                _dataAccess.PlantType_Update(pt);
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

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
