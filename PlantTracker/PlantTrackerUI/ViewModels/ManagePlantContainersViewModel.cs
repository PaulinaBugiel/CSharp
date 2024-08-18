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
    internal class ManagePlantContainersViewModel: PopupViewModelBase, IManagePlantAttributes<PlantContainer>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<PlantContainer> _plantContainersToView;
        private readonly ObservableCollection<PlantContainer> _plantContainersInDb;
        private ObservableCollection<PlantContainer> _tmpAdds;
        private ObservableCollection<PlantContainer> _tmpDeletes;
        private IDataAccess _dataAccess;
        string _newPlantContainerText = "";

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
            _plantContainersInDb = _dataAccess.PlantContainer_GetAll();
            _tmpAdds = new ObservableCollection<PlantContainer>();
            _tmpDeletes = new ObservableCollection<PlantContainer>();
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
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewPlantContainer()
        {
            var typeWithTheSameName = _plantContainersToView.Where(x => x.Name == NewAttributeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Watering system \"{NewAttributeText}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlantContainer plantContainerToAdd = new PlantContainer { Name = NewAttributeText };
                _plantContainersToView.Add(plantContainerToAdd);
                _tmpAdds.Add(plantContainerToAdd);
                NewAttributeText = "";

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
            if (_tmpAdds.Count() == 0 && _tmpDeletes.Count() == 0)
                return false;
            return true;
        }
        void ApplyChanges()
        {
            foreach (var pc in _tmpDeletes)
            {
                _dataAccess.PlantContainer_DeletePlantContainer(pc.Id);
            }
            foreach (var pc in _tmpAdds)
            {
                _dataAccess.PlantContainer_InsertOne(pc);
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
