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
    internal class ManagePlantContainersViewModel: PopupViewModelBase, IManagePlantAttributes<PlantContainer>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<PlantContainer> _plantContainersToView;
        private PlantContainer _selectedContainer;
        private ObservableCollection<PlantContainer> _tmpUpdates;
        private ObservableCollection<PlantContainer> _tmpAdds;
        private ObservableCollection<PlantContainer> _tmpDeletes;
        private IDataAccess _dataAccess;

        private RelayCommand _addNewRowCommand;
        private RelayCommand _removePlantContainerCommand;
        private RelayCommand _applyChangesCommand;
        private RelayCommand _cancelChangesCommand;
        public ManagePlantContainersViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantContainersToView = _dataAccess.PlantContainer_GetAll();
            _tmpAdds = new();
            _tmpDeletes = new();
            _tmpUpdates = new();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }


        public ObservableCollection<PlantContainer> PlantAttributes
        {
            get { return _plantContainersToView; }
            set
            {
                if (_plantContainersToView == value)
                    return;
                _plantContainersToView = value;
                OnPropertyChanged(nameof(PlantAttributes));
            }
        }

        public PlantContainer SelectedAttribute
        {
            get { return _selectedContainer; }
            set
            {
                if (_selectedContainer == value)
                    return;
                _selectedContainer = value;
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
                ObservableCollection<PlantContainer> newItems = new();
                foreach (PlantContainer item in e.NewItems)
                {
                    newItems.Add(item);
                    _tmpAdds.Add(item);
                }
                SubscribeCollectionItems(newItems);

            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                ObservableCollection<PlantContainer> oldItems = new();
                foreach (var item in e.OldItems)
                    oldItems.Add(item as PlantContainer);
                UnsubscribeCollectionItems(oldItems);
            }
        }

        private void SubscribeCollectionItems(ObservableCollection<PlantContainer> items)
        {
            if (items != null)
            {
                foreach (var item in items) item.PropertyChanged += ItemChanged;
            }
        }

        private void UnsubscribeCollectionItems(ObservableCollection<PlantContainer> items)
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
                PlantContainer? container = sender as PlantContainer;
                if (e.PropertyName == "Name")
                {
                    var containerWithTheSameName = _plantContainersToView.FirstOrDefault(x => x.Name == container.Name);
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
                if (_removePlantContainerCommand == null)
                    _removePlantContainerCommand = new RelayCommand(o => RemovePlantContainer(o), o => CanRemovePlantContainer(o));
                return _removePlantContainerCommand;
            }
        }

        bool CanRemovePlantContainer(object toRemove)
        {
            if (toRemove != null) return true;
            return false;
        }
        void RemovePlantContainer(object toRemove)
        {
            if (toRemove is not PlantContainer)
                return;
            PlantContainer plantContainerToRemove = (PlantContainer)toRemove;
            var plantsThatUseTheSystem = _dataAccess.PlantContainer_GetAllPlantsWithContainer(plantContainerToRemove);
            if (plantsThatUseTheSystem.Count() > 0)
            {
                string plantsInUseString = string.Join("\n- ", plantsThatUseTheSystem.Select(x => x.Name));
                MessageBoxResult result = MessageBox.Show($"The following plants use this plant container:\n- {plantsInUseString}\nProceed?", "Plant container in use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _tmpDeletes.Add(plantContainerToRemove);
                    _plantContainersToView.Remove(plantContainerToRemove);
                    _tmpUpdates.Remove(plantContainerToRemove);
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantContainerToRemove))
                    _tmpDeletes.Add(plantContainerToRemove);
                _plantContainersToView.Remove(plantContainerToRemove);
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
                _dataAccess.PlantContainer_DeletePlantContainer(pc.Id);
            foreach (var pc in _tmpAdds)
                _dataAccess.PlantContainer_InsertOne(pc);
            foreach (var pc in _tmpUpdates) // TODO copy to other ViewModels
                _dataAccess.PlantContainer_Update(pc);
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
