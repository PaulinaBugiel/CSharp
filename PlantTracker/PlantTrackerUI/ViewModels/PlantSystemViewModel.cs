using PlantTrackerUI.DataAccess;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.ViewModels
{
    public class PlantSystemViewModel : INotifyPropertyChanged
    {
        public List<Plant> Plants { get; set; } = new List<Plant>();
        public List<PlantType> PlantTypes { get; set; } = new List<PlantType>();
        public List<PlantContainer> Containers { get; set; } = new List<PlantContainer>();
        public List<WateringSystem> WateringSystems { get; set; } = new List<WateringSystem>();
        public List<PlantPosition> Positions { get; set; } = new List<PlantPosition>();
        private readonly IDataAccess dataAccess;

        public event PropertyChangedEventHandler PropertyChanged;

        private Plant _selectedPlant;

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

        public PlantSystemViewModel(String dataAccessType)
        {
            if (dataAccessType == "Sql")
                dataAccess = new SqlDataAccess();
            else
                dataAccess = new DemoDataAccess();

            Plants = dataAccess.Plants_GetAll();
            PlantTypes = dataAccess.PlantType_GetAll();           // TODO maybe move to a specific action
            Containers = dataAccess.Container_GetAll();           // TODO maybe move to a specific action
            WateringSystems = dataAccess.WateringSystem_GetAll(); // TODO maybe move to a specific action
            Positions = dataAccess.PlantPosition_GetAll();        // TODO maybe move to a specific action
                                                                  //OnPropertyChanged(nameof(wateringSystems)); TODO where shoult it be?
                                                                  // TODO change 
                                                                  //SelectedPlant = Plants.Where(x => x.Name == "Bazylia").FirstOrDefault();

        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
