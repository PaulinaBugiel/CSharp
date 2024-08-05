using PlantTrackerUI.UserControls;
using System;
using System.Collections;
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

namespace PlantTrackerUI.Views
{
    /// <summary>
    /// Interaction logic for ManagePlantAttributesView.xaml
    /// </summary>
    public partial class ManagePlantAttributesView : Page
    {
        public IEnumerable StackPanelItemsSource
        {
            get { return (IEnumerable)GetValue(StackPanelItemsSourceProperty); }
            set { SetValue(StackPanelItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty StackPanelItemsSourceProperty =
            DependencyProperty.Register("StackPanelItemsSource", typeof(IEnumerable), typeof(ManagePlantAttributesView), new PropertyMetadata(null));

        public ManagePlantAttributesView()
        {
            InitializeComponent();
        }

        //public ICommand RemoveElementCommand
        //{
        //    get { return (ICommand)GetValue(RemoveElementCommandProperty); }
        //    set { SetValue(RemoveElementCommandProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for RemoveElementCommand.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty RemoveElementCommandProperty =
        //    DependencyProperty.Register("RemoveElementCommand", typeof(ICommand), typeof(WrapPanelItemsControl), new PropertyMetadata(null));
    }
}
