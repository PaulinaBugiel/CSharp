﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlantTrackerUI.Views
{
    /// <summary>
    /// Interaction logic for AddPlantTypeWindow.xaml
    /// </summary>
    public partial class AddPlantTypeWindow : Window
    {
        public AddPlantTypeWindow()
        {
            InitializeComponent();
        }

        private void newTypeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            newTypeTextBox.Text = "";
        }
    }
}
