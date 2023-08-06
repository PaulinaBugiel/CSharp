using HelpMeFocus.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HelpMeFocus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CounterViewModel vm = new();
            MainWindow = new MainWindow()
            {
                DataContext = vm
            };
            vm.CloseAction = new Action(MainWindow.Close);
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
