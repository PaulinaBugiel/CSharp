using PlantTrackerUI.Core;
using PlantTrackerUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.ViewModels
{
    interface IManagePlantAttributes<T> where T : IModel
    {
        public ObservableCollection<T> PlantAttributes { get; set; }
        //public Plant SelectedPlant { get; set; }
        public string NewAttributeText { get; set; }
        //public T SelectedAttribute { get; set; }
        public RelayCommand AddNewAttributeCommand { get; }
        public RelayCommand CancelAddingAttributeCommand { get; }
        public RelayCommand RemoveAttributeCommand { get; }
        public RelayCommand ApplyChangesCommand { get; }
        public RelayCommand DiscardChangesCommand { get; }
    }
}
