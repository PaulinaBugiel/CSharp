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
    class ManageWateringSystemsViewModel : PopupViewModelBase, IManagePlantAttributes<WateringSystem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<WateringSystem> _wateringSystemsToView;
        private readonly ObservableCollection<WateringSystem> _wateringSystemsInDb;
        private ObservableCollection<WateringSystem> _tmpAdds;
        private ObservableCollection<WateringSystem> _tmpDeletes;
        private IDataAccess _dataAccess;
        string _newWateringSystemText = ""; // TODO probably some container?

        private RelayCommand _addNewWateringSystemCommand;
        private RelayCommand _removeWateringSystemCommand;
        private RelayCommand _cancelAddingWateringSystemCommand;
        private RelayCommand _applyChangesCommand;

        public ManageWateringSystemsViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"];
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _wateringSystemsToView = _dataAccess.WateringSystem_GetAll();
            _wateringSystemsInDb = _dataAccess.WateringSystem_GetAll();
            _tmpAdds = new ObservableCollection<WateringSystem>();
            _tmpDeletes = new ObservableCollection<WateringSystem>();
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

        public string NewAttributeText
        {
            get { return _newWateringSystemText; }
            set
            {
                if (_newWateringSystemText == value)
                    return;
                _newWateringSystemText = value;
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
                if (_addNewWateringSystemCommand == null)
                    _addNewWateringSystemCommand = new RelayCommand(o => AddNewWateringSystem(), o => CanAddNewWateringSystem());
                return _addNewWateringSystemCommand;
            }
        }
        bool CanAddNewWateringSystem()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewWateringSystem()
        {
            var typeWithTheSameName = _wateringSystemsToView.Where(x => x.Name == NewAttributeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Watering system \"{NewAttributeText}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WateringSystem wateringSystemToAdd = new WateringSystem { Name = NewAttributeText };
                _wateringSystemsToView.Add(wateringSystemToAdd);
                _tmpAdds.Add(wateringSystemToAdd);
                //WateringSystem? maxIdType = _wateringSystemsToView.OrderByDescending(x => x.Id).First();
                //int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                //WateringSystem newWateringSystem = new WateringSystem { Name = NewAttributeText, Id = newId };
                NewAttributeText = "";
                //_dataAccess.WateringSystem_InsertOne(newWateringSystem);
                //PlantAttributes = _dataAccess.WateringSystem_GetAll();
                // // OnPropertyChanged(nameof(PlantAttributes));

            }
        }

        public RelayCommand RemoveAttributeCommand
        {
            get
            {
                if (_removeWateringSystemCommand == null)
                    _removeWateringSystemCommand = new RelayCommand(o => RemoveWateringSystem(), o => CanRemoveWateringSystem());
                return _removeWateringSystemCommand;
            }
        }

        bool CanRemoveWateringSystem()
        {
            return true;
        }
        void RemoveWateringSystem(/*object toRemove*/)
        {
            //if (toRemove is not WateringSystem)
            //    return;
            //WateringSystem wateringSystemToRemove = (WateringSystem)toRemove;
            MessageBox.Show($"Name of the Watering System:");
            // TODO try removing from _tmpAdds
            // TODO try removing from DB
            // TODO remove from ...ToView
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
            // TODO delete records from DB (_tmpDeletes)
            // TODO add records to db (_tmpAdds)
        }

        public RelayCommand DiscardChangesCommand => throw new NotImplementedException(); // TODO is it really needed?

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
