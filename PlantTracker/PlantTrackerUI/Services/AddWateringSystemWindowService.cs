using PlantTrackerUI.ViewModels;
using PlantTrackerUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Services
{
    internal class AddWateringSystemWindowService : IWindowService
    {
        private AddWateringSystemWindow? _window;
        public void ShowWindow(PopupViewModelBase dataContext)
        {
            // TODO try to use implicit datatemplates (https://stackoverflow.com/questions/25845689/opening-new-window-in-mvvm-wpf?rq=1)
            _window = new AddWateringSystemWindow();
            dataContext.CloseAction = _window.Close;
            _window.DataContext = dataContext;
            _window.ShowDialog();
        }

        public void CloseWindow()
        {
            _window.Close();
        }
    }
}
