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
    internal class ManagePlantPositionsViewModel: PopupViewModelBase, IManagePlantAttributes<PlantPosition>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<PlantPosition> _plantPositionsToView;
        private PlantPosition _selectedPlantPosition;
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
            _tmpAdds = new ObservableCollection<PlantPosition>();
            _tmpDeletes = new ObservableCollection<PlantPosition>();
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

        public string NewAttributePlaceholderText { get { return "New Plant Position:"; } set { } }

        public string NewAttributeText
        {
            get { return _newPlantPositionText; }
            set
            {
                if (_newPlantPositionText == value)
                    return;
                _newPlantPositionText = value;
                OnPropertyChanged(nameof(NewAttributeText));
            }
        }


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

        }


        /// <summary>
        /// Command invoked when a row editing is finished
        /// </summary>
        public RelayCommand FinishedEditingRowCommand
        {
            get
            {
                if (_finishedEditingRowCommand == null)
                    _finishedEditingRowCommand = new RelayCommand(o => OnRowEditingFinished());
                return _finishedEditingRowCommand;
            }
        }

        void OnRowEditingFinished()
        {
            MessageBox.Show(_selectedPlantPosition.Name);
            // TODO edit/update
        }

        /// <summary>
        /// Adds new PlantPosition with name written in NewPlantPositionText field
        /// </summary>
        public RelayCommand AddNewAttributeCommand
        {
            get
            {
                if (_addNewPlantPositionCommand == null)
                    _addNewPlantPositionCommand = new RelayCommand(o => AddNewPlantPosition(), o => CanAddNewPlantPosition());
                return _addNewPlantPositionCommand;
            }
        }
        bool CanAddNewPlantPosition()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewPlantPosition()
        {
            var typeWithTheSameName = _plantPositionsToView.Where(x => x.Name == NewAttributeText).FirstOrDefault();
            if (typeWithTheSameName is not null)
            {
                MessageBox.Show($"Watering system \"{NewAttributeText}\" already exists", "Cannot add",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlantPosition plantPositionToAdd = new PlantPosition { Name = NewAttributeText };
                _plantPositionsToView.Add(plantPositionToAdd);
                _tmpAdds.Add(plantPositionToAdd);
                NewAttributeText = "";

            }
        }


        /// <summary>
        /// Cancels adding new PlantPosition - clears the NewPlantPositionText field
        /// </summary>
        public RelayCommand CancelAddingAttributeCommand
        {
            get
            {
                if (_cancelAddingPlantPositionCommand == null)
                    _cancelAddingPlantPositionCommand = new RelayCommand(o => CancelAddingPlantPosition(), o => CanCancelAddingPlantPosition());
                return _cancelAddingPlantPositionCommand;
            }
        }
        bool CanCancelAddingPlantPosition()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingPlantPosition()
        {
            NewAttributeText = "";
        }

        public RelayCommand RemoveAttributeCommand
        {
            get
            {
                if (_removePlantPositionCommand == null)
                    _removePlantPositionCommand = new RelayCommand(o => RemovePlantPosition(o), o => CanRemovePlantPosition());
                return _removePlantPositionCommand;
            }
        }

        bool CanRemovePlantPosition()
        {
            return true;
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
                }
            }
            else
            {
                if (!_tmpAdds.Remove(plantPositionToRemove))
                    _tmpDeletes.Add(plantPositionToRemove);
                _plantPositionsToView.Remove(plantPositionToRemove);
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
                _dataAccess.PlantPosition_DeletePlantPosition(ws.Id);
            }
            foreach (var ws in _tmpAdds)
            {
                _dataAccess.PlantPosition_InsertOne(ws);
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

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
