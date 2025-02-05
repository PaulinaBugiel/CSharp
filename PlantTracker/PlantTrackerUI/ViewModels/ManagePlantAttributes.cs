using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.ViewModels
{
    internal class ManagePlantAttributes<T> : PopupViewModelBase, IManagePlantAttributes<T> where T : IModel, INotifyPropertyChanged, new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected ObservableCollection<T> _plantAttributes;
        protected ObservableCollection<T> _tmpUpdates;
        protected ObservableCollection<T> _tmpAdds;
        protected ObservableCollection<T> _tmpDeletes;
        T _selectedAttribute;

        protected RelayCommand _addNewRowCommand;
        protected RelayCommand _removeAttributeCommand;
        private RelayCommand _applyChangesCommand;
        private RelayCommand _cancelChangesCommand;

        public ManagePlantAttributes()
        {
            _tmpAdds = new();
            _tmpDeletes = new();
            _tmpUpdates = new();
        }

        public ObservableCollection<T> PlantAttributes
        {
            get { return _plantAttributes; }
            set
            {
                if (_plantAttributes == value)
                    return;
                _plantAttributes = value;
                OnPropertyChanged(nameof(PlantAttributes));
            }
        }
        public T SelectedAttribute
        {
            get { return _selectedAttribute; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_selectedAttribute, value))
                    return;
                _selectedAttribute = value;
                OnPropertyChanged(nameof(SelectedAttribute));
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
                if (_removeAttributeCommand == null)
                    _removeAttributeCommand = new RelayCommand(o => RemoveAttribute(o), o => CanRemoveAttribute(o));
                return _removeAttributeCommand;
            }
        }

        bool CanRemoveAttribute(object toRemove)
        {
            if (toRemove != null) return true;
            return false;
        }
        void RemoveAttribute(object toRemove)
        {
            if (toRemove is not T)
                return;
            T attributeToRemove = (T)toRemove;
            var plantsThatUseTheSystem = DataAccess_GetAllPlantsWithAttribute(attributeToRemove);
            if (plantsThatUseTheSystem.Count() > 0)
            {
                string plantsInUseString = string.Join("\n- ", plantsThatUseTheSystem.Select(x => x.Name));

                // TODO think of the string building appropriate to type T
                MessageBoxResult result = MessageBox.Show($"The following plants use this plant container:\n- {plantsInUseString}\nProceed?", "Plant container in use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    _tmpDeletes.Add(attributeToRemove);
                    _plantAttributes.Remove(attributeToRemove);
                    _tmpUpdates.Remove(attributeToRemove);
                }
            }
            else
            {
                if (!_tmpAdds.Remove(attributeToRemove))
                    _tmpDeletes.Add(attributeToRemove);
                _plantAttributes.Remove(attributeToRemove);
                _tmpUpdates.Remove(attributeToRemove);
            }
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

        /// <summary>
        /// Fired when an element is added to DataGrid or deleted via 'Delete' button (either of these actions has to be enabled in DataGrid)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains OldItems - deleted items, and NewItems - newly added items</param>
        public void OnPlantAttributesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null && e.NewItems.Count > 0)
            {
                ObservableCollection<T> newItems = new();
                foreach (T item in e.NewItems)
                {
                    newItems.Add(item);
                    _tmpAdds.Add(item);
                }
                SubscribeCollectionItems(newItems);

            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                ObservableCollection<T> oldItems = new();
                foreach (var item in e.OldItems)
                    oldItems.Add((T)item);
                UnsubscribeCollectionItems(oldItems);
            }
        }

        protected void SubscribeCollectionItems(ObservableCollection<T> items)
        {
            if (items != null)
            {
                foreach (var item in items) item.PropertyChanged += ItemChanged;
            }
        }

        private void UnsubscribeCollectionItems(ObservableCollection<T> items)
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
                T? attribute = (T)sender;
                if (e.PropertyName == "Name")
                {
                    var attributeWithTheSameName = _plantAttributes.FirstOrDefault(x => x.Name == attribute.Name);
                    if (attributeWithTheSameName is not null && !EqualityComparer<T>.Default.Equals(attributeWithTheSameName, attribute))
                    {
                        MessageBox.Show($"Plant attribute \"{attribute.Name}\" already exists", "Name duplicate",
                            MessageBoxButton.OK);
                    }
                }
                // If the attribute is already in newly added or updated attributes, there's no need to add it again
                if (!(_tmpUpdates.Contains(attribute) || _tmpAdds.Contains(attribute)))
                    _tmpUpdates.Add(attribute);
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
            foreach (var pc in _tmpDeletes)
                DataAccess_DeleteAttribute(pc.Id);
            foreach (var pc in _tmpAdds)
                DataAccess_InsertAttribute(pc);
            foreach (var pc in _tmpUpdates)
                DataAccess_UpdateAttribute(pc);
            CloseWindow();
        }


        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Function to be overriden in child classes. Retrieves all plants that use (are related to) the gitven attribute
        /// </summary>
        /// <param name="attribute">attribute by which to search for all plants that use it (eg. PlantContainer, PlantType etc.)</param>
        /// <returns>ObservableCollection of Plants</returns>
        protected virtual ObservableCollection<Plant> DataAccess_GetAllPlantsWithAttribute(T attribute)
        {
            return new ObservableCollection<Plant>();
        }

        protected virtual void DataAccess_DeleteAttribute(int id) { }
        protected virtual void DataAccess_InsertAttribute(T attribute) { }
        protected virtual void DataAccess_UpdateAttribute(T attribute) { }
    }
}
