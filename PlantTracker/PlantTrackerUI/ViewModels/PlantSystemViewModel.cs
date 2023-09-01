using PlantTrackerUI.Core;
using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using PlantTrackerUI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.ViewModels
{
    public class PlantSystemViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public List<Plant> PlantsList { get; set; } = new List<Plant>();
        //public List<PlantType> PlantTypes { get; set; } = new List<PlantType>();
        //public List<PlantContainer> Containers { get; set; } = new List<PlantContainer>();
        //public List<WateringSystem> WateringSystems { get; set; } = new List<WateringSystem>();
        //public List<PlantPosition> Positions { get; set; } = new List<PlantPosition>();
        private readonly IDataAccess _dataAccess;

        private Plant _selectedPlant;

        private RelayCommand _openAddPlantTypeWindowCommand;

        public PlantSystemViewModel()
        {
            string dataSourceType = ConfigurationManager.AppSettings["DataSource"]; // TODO test for null
            if (dataSourceType == "Sql")
                _dataAccess = new SqlDataAccess();
            else
                _dataAccess = new DemoDataAccess();

            PlantsList = _dataAccess.Plants_GetAll();
            //PlantTypes = dataAccess.PlantType_GetAll();           // TODO maybe move to a specific action
            //Containers = dataAccess.Container_GetAll();           // TODO maybe move to a specific action
            //WateringSystems = dataAccess.WateringSystem_GetAll(); // TODO maybe move to a specific action
            //Positions = dataAccess.PlantPosition_GetAll();        // TODO maybe move to a specific action
                                                                  //OnPropertyChanged(nameof(wateringSystems)); TODO where shoult it be?
                                                                  // TODO change 
                                                                  //SelectedPlant = Plants.Where(x => x.Name == "Bazylia").FirstOrDefault();

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


        public bool CanOpenAddPlantTypeWindow()
        {
            return true;
        }
        public void OpenAddPlantTypeWindow(Plant plant)
        {
            // TODO this breaks MVVM, try IWindowService od IDialogWindowService
            // TODO block the parent window
            // TODO prevent from opening more than once
            Window addPlantTypeWindow = new AddPlantTypeWindow
            {
                DataContext = new AddPlantTypeViewModel(SelectedPlant)
            };
            addPlantTypeWindow.Show();
        }
        public RelayCommand OpenAddPlantTypeWindowCommand
        {
            get
            {
                if (_openAddPlantTypeWindowCommand == null)
                    _openAddPlantTypeWindowCommand = new RelayCommand(o => OpenAddPlantTypeWindow(SelectedPlant), o => CanOpenAddPlantTypeWindow());
                return _openAddPlantTypeWindowCommand;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
