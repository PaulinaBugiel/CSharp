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
    class ManageWateringSystemsViewModel : PopupViewModelBase, IManagePlantAttributes<WateringSystem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<WateringSystem> _wateringSystemsToView;
        private WateringSystem _selectedWateringSystem;
        private readonly ObservableCollection<WateringSystem> _wateringSystemsInDb;
        private ObservableCollection<WateringSystem> _tmpUpdates;
        private ObservableCollection<WateringSystem> _tmpAdds;
        private ObservableCollection<WateringSystem> _tmpDeletes;
        private IDataAccess _dataAccess;
        string _newWateringSystemText = "";

        private RelayCommand _addNewRowCommand;
        private RelayCommand _finishedEditingRowCommand;
        private RelayCommand _addNewWateringSystemCommand;
        private RelayCommand _removeWateringSystemCommand;
        private RelayCommand _cancelAddingWateringSystemCommand;
        private RelayCommand _applyChangesCommand;
        private RelayCommand _cancelChangesCommand;

        public ManageWateringSystemsViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _wateringSystemsToView = _dataAccess.WateringSystem_GetAll();
            _wateringSystemsInDb = _dataAccess.WateringSystem_GetAll();
            _tmpAdds = new();
            _tmpDeletes = new();
            _tmpUpdates = new();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }


        public ObservableCollection<WateringSystem> PlantAttributes 
        { 
            get { return _wateringSystemsToView; }
            set
            {
                if (_wateringSystemsToView == value)
                    return;
                _wateringSystemsToView = value;
                OnPropertyChanged(nameof(PlantAttributes));
            }
        }

        public WateringSystem SelectedAttribute
        {
            get { return _selectedWateringSystem; }
            set
            {
                if (_selectedWateringSystem == value)
                    return;
                _selectedWateringSystem = value;
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
                ObservableCollection<WateringSystem> newItems = new();
                foreach (WateringSystem item in e.NewItems)
                {
                    newItems.Add(item);
                    _tmpAdds.Add(item);
                }
                SubscribeCollectionItems(newItems);

            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                ObservableCollection<WateringSystem> oldItems = new();
                foreach (var item in e.OldItems)
                    oldItems.Add(item as WateringSystem);
                UnsubscribeCollectionItems(oldItems);
            }
        }

        private void SubscribeCollectionItems(ObservableCollection<WateringSystem> items)
        {
            if (items != null)
            {
                foreach (var item in items) item.PropertyChanged += ItemChanged;
            }
        }

        private void UnsubscribeCollectionItems(ObservableCollection<WateringSystem> items)
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
                WateringSystem? container = sender as WateringSystem;
                if (e.PropertyName == "Name")
                {
                    var containerWithTheSameName = _wateringSystemsToView.FirstOrDefault(x => x.Name == container.Name);
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


        /// <summary>
        /// Command for adding new row to a DataGrid
        /// </summary>
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
                if (_removeWateringSystemCommand == null)
                    _removeWateringSystemCommand = new RelayCommand(o => RemoveWateringSystem(o), o => CanRemoveWateringSystem(o));
                return _removeWateringSystemCommand;
            }
        }

        bool CanRemoveWateringSystem(object toRemove)
        {
            if (toRemove != null) return true;
            return false;
        }
        void RemoveWateringSystem(object toRemove)
        {
            if (toRemove is not WateringSystem)
                return;
            WateringSystem plantContainerToRemove = (WateringSystem)toRemove;
            var plantsThatUseTheSystem = _dataAccess.WateringSystem_GetAllPlantsWithSystem(plantContainerToRemove);
            if (plantsThatUseTheSystem.Count() > 0)
            {
                string plantsInUseString = string.Join("\n- ", plantsThatUseTheSystem.Select(x => x.Name));
                MessageBoxResult result = MessageBox.Show($"The following plants use this plant container:\n- {plantsInUseString}\nProceed?", "Plant container in use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _tmpDeletes.Add(plantContainerToRemove);
                    _wateringSystemsToView.Remove(plantContainerToRemove);
                    _tmpUpdates.Remove(plantContainerToRemove);
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantContainerToRemove))
                    _tmpDeletes.Add(plantContainerToRemove);
                _wateringSystemsToView.Remove(plantContainerToRemove);
                _tmpUpdates.Remove(plantContainerToRemove);
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
            if (_tmpUpdates.Count() > 0 || _tmpAdds.Count() > 0 || _tmpDeletes.Count() > 0) // TODO copy to other ViewModels
                return true;
            return false;
        }
        void ApplyChanges()
        {
            foreach (var pc in _tmpDeletes)
                _dataAccess.WateringSystem_DeleteWateringSystem(pc.Id);
            foreach (var pc in _tmpAdds)
                _dataAccess.WateringSystem_InsertOne(pc);
            foreach (var pc in _tmpUpdates) // TODO copy to other ViewModels
                _dataAccess.WateringSystem_Update(pc);
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
