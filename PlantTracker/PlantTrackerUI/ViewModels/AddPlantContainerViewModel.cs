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
    class AddPlantContainerViewModel : PopupViewModelBase, IAddPlantAttribute<PlantContainer>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<PlantContainer> _availablePlantContainers;
        private readonly IDataAccess _dataAccess;
        private Plant _selectedPlant;
        private string _newContainerText = "";

        private PlantContainer? _selectedContainer;


        private RelayCommand? _addNewContainerCommand;
        private RelayCommand? _cancelAddingContainerCommand;
        private RelayCommand? _addSelectedContainerCommand;


        public AddPlantContainerViewModel(Plant selectedPlant)
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();
            _selectedPlant = selectedPlant;
            _availablePlantContainers = _dataAccess.PlantContainer_GetAvailableForPlant(_selectedPlant.Id);
        }

        #region Properties

        public ObservableCollection<PlantContainer> AvailablePlantAttributes
        {
            get { return _availablePlantContainers; }
            set
            {
                if (_availablePlantContainers == value)
                    return;
                _availablePlantContainers = value;
                OnPropertyChanged(nameof(AvailablePlantAttributes));
            }
        }

        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set
            {
                if (_selectedPlant == value)
                    return;
                _selectedPlant = value;
                OnPropertyChanged(nameof(SelectedPlant));
            }
        }

        public string NewAttributeText
        {
            get
            {
                return _newContainerText;
            }
            set
            {
                if (_newContainerText == value)
                    return;
                _newContainerText = value;
                OnPropertyChanged(nameof(NewAttributeText));
            }
        }
        public string AddSelectedAttributeButtonText { get { return "Add Selected Container"; } set { } }
        public string NewAttributeLabelText { get { return "New Container:"; } set { } }

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
        #endregion

        #region Commands

        /// <summary>
        /// Adds new PlantContainer with name from NewAttributeText field
        /// </summary>
        public RelayCommand AddNewAttributeCommand
        {
            get
            {
                if (_addNewContainerCommand == null)
                    _addNewContainerCommand = new RelayCommand(o => AddNewContainer(), o => CanAddNewContainer());
                return _addNewContainerCommand;
            }
        }
        bool CanAddNewContainer()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void AddNewContainer()
        {
            ObservableCollection<PlantContainer> allContainers = _dataAccess.PlantContainer_GetAll();
            var containerWithTheSameName = allContainers.Where(x => x.Name == NewAttributeText).FirstOrDefault();
            if (containerWithTheSameName is not null)
            {
                MessageBox.Show($"Plant container \"{NewAttributeText}\" already exists", "Cannot add plant container",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlantContainer? maxIdType = allContainers.OrderByDescending(x => x.Id).First();
                int newId = maxIdType is null ? 1 : maxIdType.Id + 1;
                PlantContainer newContainer = new PlantContainer { Name = NewAttributeText, Id = newId };
                NewAttributeText = "";
                _dataAccess.PlantContainer_InsertOne(newContainer);
                AvailablePlantAttributes = _dataAccess.PlantContainer_GetAvailableForPlant(_selectedPlant.Id);

            }
        }

        /// <summary>
        /// Cancels adding new PlantType - clears the NewTypeText field
        /// </summary>
        public RelayCommand CancelAddingAttributeCommand
        {
            get
            {
                if (_cancelAddingContainerCommand == null)
                    _cancelAddingContainerCommand = new RelayCommand(o => CancelAddingContainer(), o => CanCancelAddingContainer());
                return _cancelAddingContainerCommand;
            }
        }
        bool CanCancelAddingContainer()
        {
            if (NewAttributeText.Length > 0)
                return true;
            else
                return false;
        }
        void CancelAddingContainer()
        {
            NewAttributeText = "";
        }

        public RelayCommand AddSelectedAttributeCommand
        {
            get
            {
                if (_addSelectedContainerCommand == null)
                    _addSelectedContainerCommand = new RelayCommand(o => AddSelectedContainer(), o => CanAddSelectedContainer());
                return _addSelectedContainerCommand;
            }
        }
        bool CanAddSelectedContainer()
        {
            if (SelectedAttribute is null)
                return false;
            else
                return true;
        }
        void AddSelectedContainer()
        {
            SelectedPlant.Containers.Add(SelectedAttribute);
            _dataAccess.PlantContainer_AddOneForPlant(SelectedPlant.Id, SelectedAttribute.Id);
            OnPropertyChanged(nameof(SelectedPlant));
            CloseWindow();
            
        }
        #endregion

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
