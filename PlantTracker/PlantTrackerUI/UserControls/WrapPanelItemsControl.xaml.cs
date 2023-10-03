using PlantTrackerUI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PlantTrackerUI.UserControls
{
    /// <summary>
    /// Interaction logic for WrapPanelItemsControl.xaml
    /// </summary>
    public partial class WrapPanelItemsControl : UserControl
    {
        public string AddButtonContent
        {
            get { return (string)GetValue(AddButtonContentProperty); }
            set { SetValue(AddButtonContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for AddButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonContentProperty =
            DependencyProperty.Register("AddButtonContent", typeof(string), typeof(WrapPanelItemsControl), new PropertyMetadata("Add..."));



        public IEnumerable WrapPanelItemsSource
        {
            get { return (IEnumerable)GetValue(WrapPanelItemsSourceProperty); }
            set { SetValue(WrapPanelItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty WrapPanelItemsSourceProperty =
            DependencyProperty.Register("WrapPanelItemsSource", typeof(IEnumerable), typeof(WrapPanelItemsControl), new PropertyMetadata(null));



        public ICommand AddElementWindowCommand
        {
            get { return (ICommand)GetValue(AddElementWindowCommandProperty); }
            set { SetValue(AddElementWindowCommandProperty, value); }
        }
        public static readonly DependencyProperty AddElementWindowCommandProperty =
            DependencyProperty.Register("AddElementWindowCommand", typeof(ICommand), typeof(WrapPanelItemsControl), new PropertyMetadata(null));



        public ICommand RemoveElementCommand
        {
            get { return (ICommand)GetValue(RemoveElementCommandProperty); }
            set { SetValue(RemoveElementCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveElementCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveElementCommandProperty =
            DependencyProperty.Register("RemoveElementCommand", typeof(ICommand), typeof(WrapPanelItemsControl), new PropertyMetadata(null));




        //public static readonly RoutedEvent RemoveItemEvent = EventManager.RegisterRoutedEvent(nameof(RemoveItem), RoutingStrategy.Bubble, typeof(), typeof(WrapPanelItemsControl));
        //public event RoutedEventHandler RemoveItem
        //{
        //    add { AddHandler(RemoveItemEvent, value); }
        //    remove { RemoveHandler(RemoveItemEvent, value); }
        //}


        public WrapPanelItemsControl()
        {
            InitializeComponent();
        }

        //private void OnRemoveItem(object sender, RoutedEventArgs e)
        //{
        //    RaiseEvent(new RoutedEventArgs(RemoveItemEvent));
        //}
    }
}
