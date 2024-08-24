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
        string _newPlantContainerText = "";
        private PlantContainer? _newlyAddedContainer;

        private RelayCommand _addNewRowCommand;
        private RelayCommand _finishedEditingRowCommand;
        private RelayCommand _addNewPlantContainerCommand;
        private RelayCommand _removePlantContainerCommand;
        private RelayCommand _cancelAddingPlantContainerCommand;
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
            _tmpAdds = new(); // TODO copy to other ViewModels
            _tmpDeletes = new(); // TODO copy to other ViewModels
            _tmpUpdates = new();
            _newlyAddedContainer = null;
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
                _selectedContainer = value; // TODO id _selectedContainer is not null, look for it in _plantContainersToView and check for changes
                OnPropertyChanged(nameof(SelectedAttribute));
            }
        }

        public string NewAttributePlaceholderText { get { return "New Container:"; } set { } }

        public string NewAttributeText
        {
            get { return _newPlantContainerText; }
            set
            {
                if (_newPlantContainerText == value)
                    return;
                _newPlantContainerText = value;
                OnPropertyChanged(nameof(NewAttributeText));
            }
        }

        void OnPlantAttributesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeCollectionItems(e.OldItems as ObservableCollection<PlantContainer>);
            SubscribeCollectionItems(e.NewItems as ObservableCollection<PlantContainer>);
            ObservableCollection<PlantContainer> senderCollection = sender as ObservableCollection<PlantContainer>;
            NotifyCollectionChangedAction action = e.Action;
        }

        private void SubscribeCollectionItems(ObservableCollection<PlantContainer> items)
        {
            foreach (var item in items)
            {
                item.PropertyChanged += ItemChanged;
            }
        }

        private void UnsubscribeCollectionItems(ObservableCollection<PlantContainer> items)
        {
            foreach (var item in items)
            {
                item.PropertyChanged -= ItemChanged;
            }
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender != null)
            {
                PlantContainer? containter = sender as PlantContainer;
                _tmpUpdates.Add(containter);
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
                    _addNewRowCommand = new RelayCommand(o => AddNewRow(), o => CanAddNewRow());
                return _addNewRowCommand;
            }
        }
        bool CanAddNewRow()
        {
            return true;
        }
        void AddNewRow()
        {
            _newlyAddedContainer = new();
            PlantAttributes.Add(_newlyAddedContainer);
        }


        /// <summary>
        /// Command invoked when a row editing is finished
        /// </summary>
        public RelayCommand FinishedEditingRowCommand
        {
            get
            {
                if (_finishedEditingRowCommand == null)
                    _finishedEditingRowCommand = new RelayCommand(o => OnRowEditingFinished(o));
                return _finishedEditingRowCommand;
            }
        }
        
        void OnRowEditingFinished(object parameter)
        {
            MessageBox.Show(_selectedContainer.Name);
            // TODO edit/update
        }

        /// <summary>
        /// Adds new PlantContainer with name written in NewPlantContainerText field
        /// </summary>
        public RelayCommand AddNewAttributeCommand
        {
            get
            {
                if (_addNewPlantContainerCommand == null)
                    _addNewPlantContainerCommand = new RelayCommand(o => AddNewPlantContainer(), o => CanAddNewPlantContainer());
                return _addNewPlantContainerCommand;
            }
        }
        bool CanAddNewPlantContainer()
        {
            
            if (_newlyAddedContainer == null || _newlyAddedContainer.Name == "")
                return false;
            return true;
            //if (NewAttributeText.Length > 0)
            //    return true;
            //else
            //    return false;
        }
        void AddNewPlantContainer()
        {
            if (_newlyAddedContainer == null)
                return;
            var containerWithTheSameName = _plantContainersToView.FirstOrDefault(x => x.Name == _newlyAddedContainer.Name);
            if (containerWithTheSameName is not null && containerWithTheSameName != _newlyAddedContainer)
            {
                MessageBox.Show($"Watering system \"{_newlyAddedContainer.Name}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //_plantContainersToView.Add(_newlyAddedContainer);
                _tmpAdds.Add(_newlyAddedContainer);
                _newlyAddedContainer = null;
            }
        }


        /// <summary>
        /// Cancels adding new PlantContainer - clears the NewPlantContainerText field
        /// </summary>
        public RelayCommand CancelAddingAttributeCommand
        {
            get
            {
                if (_cancelAddingPlantContainerCommand == null)
                    _cancelAddingPlantContainerCommand = new RelayCommand(o => CancelAddingPlantContainer(), o => CanCancelAddingPlantContainer());
                return _cancelAddingPlantContainerCommand;
            }
        }
        bool CanCancelAddingPlantContainer()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingPlantContainer()
        {
            NewAttributeText = "";
        }

        public RelayCommand RemoveAttributeCommand
        {
            get
            {
                if (_removePlantContainerCommand == null)
                    _removePlantContainerCommand = new RelayCommand(o => RemovePlantContainer(o), o => CanRemovePlantContainer());
                return _removePlantContainerCommand;
            }
        }

        bool CanRemovePlantContainer()
        {
            return true;
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
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantContainerToRemove))
                    _tmpDeletes.Add(plantContainerToRemove);
                _plantContainersToView.Remove(plantContainerToRemove);
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
