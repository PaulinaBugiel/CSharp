using PlantTrackerUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.Services
{
    internal class AddTypeWindowService : IWindowService
    {

        public void ShowWindow(object dataContext)
        {
            // TODO try to use implicit datatemplates (https://stackoverflow.com/questions/25845689/opening-new-window-in-mvvm-wpf?rq=1)
            var window = new AddPlantTypeWindow();
            window.DataContext = dataContext;
            window.ShowDialog();
        }
    }
}
