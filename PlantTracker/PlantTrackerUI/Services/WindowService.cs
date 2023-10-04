using PlantTrackerUI.ViewModels;
using PlantTrackerUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantTrackerUI.Services
{
    class WindowService : IWindowService
    {
        public void CloseWindow()
        {
            //_window.Close();
        }

        public void ShowWindow(PopupViewModelBase dataContext)
        {
            var window = new Window();
            window.Width = 350;
            window.Height = 300;
            dataContext.CloseAction = window.Close;
            window.Content = dataContext;
            window.ShowDialog();
        }
    }
}
