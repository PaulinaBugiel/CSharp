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
    internal class ManagePlantPositionsViewModel: PopupViewModelBase, IManagePlantAttributes<PlantPosition>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<PlantPosition> _plantPositionsToView;
        private PlantPosition _selectedPlantPosition;
        private ObservableCollection<PlantPosition> _tmpUpdates;
        private readonly ObservableCollection<PlantPosition> _plantPositionsInDb;
        private ObservableCollection<PlantPosition> _tmpAdds;
        private ObservableCollection<PlantPosition> _tmpDeletes;
        private IDataAccess _dataAccess;
        string _newPlantPositionText = "";

        private RelayCommand _addNewRowCommand;
        private RelayCommand _finishedEditingRowCommand;
        private RelayCommand _addNewPlantPositionCommand;
        private RelayCommand _removePlantPositionCommand;
        private RelayCommand _cancelAddingPlantPositionCommand;
        private RelayCommand _applyChangesCommand;
        private RelayCommand _cancelChangesCommand;
        public ManagePlantPositionsViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _plantPositionsToView = _dataAccess.PlantPosition_GetAll();
            _plantPositionsInDb = _dataAccess.PlantPosition_GetAll();
            _tmpAdds = new();
            _tmpDeletes = new();
            _tmpUpdates = new();
            SubscribeCollectionItems(PlantAttributes);
            PlantAttributes.CollectionChanged += OnPlantAttributesChanged;
        }


        public ObservableCollection<PlantPosition> PlantAttributes
        {
            get { return _plantPositionsToView; }
            set
            {
                if (_plantPositionsToView == value)
                    return;
                _plantPositionsToView = value;
                OnPropertyChanged(nameof(PlantAttributes));
            }
        }

        public PlantPosition SelectedAttribute
        {
            get { return _selectedPlantPosition; }
            set
            {
                if (_selectedPlantPosition == value)
                    return;
                _selectedPlantPosition = value;
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
                ObservableCollection<PlantPosition> newItems = new();
                foreach (PlantPosition item in e.NewItems)
                {
                    newItems.Add(item);
                    _tmpAdds.Add(item);
                }
                SubscribeCollectionItems(newItems);

            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                ObservableCollection<PlantPosition> oldItems = new();
                foreach (var item in e.OldItems)
                    oldItems.Add(item as PlantPosition);
                UnsubscribeCollectionItems(oldItems);
            }
        }

        private void SubscribeCollectionItems(ObservableCollection<PlantPosition> items)
        {
            if (items != null)
            {
                foreach (var item in items) item.PropertyChanged += ItemChanged;
            }
        }

        private void UnsubscribeCollectionItems(ObservableCollection<PlantPosition> items)
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
                PlantPosition? container = sender as PlantPosition;
                if (e.PropertyName == "Name")
                {
                    var containerWithTheSameName = _plantPositionsToView.FirstOrDefault(x => x.Name == container.Name);
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
                if (_removePlantPositionCommand == null)
                    _removePlantPositionCommand = new RelayCommand(o => RemovePlantPosition(o), o => CanRemovePlantPosition(o));
                return _removePlantPositionCommand;
            }
        }

        bool CanRemovePlantPosition(object toRemove)
        {
            if (toRemove != null) return true;
            return false;
        }

        void RemovePlantPosition(object toRemove)
        {
            if (toRemove is not PlantPosition)
                return;
            PlantPosition plantPositionToRemove = (PlantPosition)toRemove;
            var plantsThatUseTheSystem = _dataAccess.PlantPosition_GetAllPlantsWithPosition(plantPositionToRemove);
            if (plantsThatUseTheSystem.Count() > 0)
            {
                string plantsInUseString = string.Join("\n- ", plantsThatUseTheSystem.Select(x => x.Name));
                MessageBoxResult result = MessageBox.Show($"The following plants use this plant position:\n- {plantsInUseString}\nProceed?", "Plant position in use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _tmpDeletes.Add(plantPositionToRemove);
                    _plantPositionsToView.Remove(plantPositionToRemove);
                    _tmpUpdates.Remove(plantPositionToRemove);
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantPositionToRemove))
                    _tmpDeletes.Add(plantPositionToRemove);
                _plantPositionsToView.Remove(plantPositionToRemove);
                _tmpUpdates.Remove(plantPositionToRemove);
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
            foreach (var pp in _tmpDeletes)
                _dataAccess.PlantPosition_DeletePlantPosition(pp.Id);
            foreach (var pp in _tmpAdds)
                _dataAccess.PlantPosition_InsertOne(pp);
            foreach (var pp in _tmpUpdates)
                _dataAccess.PlantPosition_Update(pp);
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
