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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelpMeFocus.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }
        private void hoursTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            hoursTextBox.Text = "";
        }

        private void minutesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            minutesTextBox.Text = "";
        }

        private void secondsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            secondsTextBox.Text = "";
        }
    }
}
