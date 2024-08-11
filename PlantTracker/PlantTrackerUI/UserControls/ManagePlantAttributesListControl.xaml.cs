using PlantTrackerUI.Views;
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

namespace PlantTrackerUI.UserControls
{
    /// <summary>
    /// Interaction logic for ManagePlantAttributesListControl.xaml
    /// </summary>
    public partial class ManagePlantAttributesListControl : UserControl
    {
        public ManagePlantAttributesListControl()
        {
            InitializeComponent();
        }

        public IEnumerable PlantAttributesItemsSource
        {
            get { return (IEnumerable)GetValue(PlantAttributesItemsSourceProperty); }
            set { SetValue(PlantAttributesItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty PlantAttributesItemsSourceProperty =
            DependencyProperty.Register("PlantAttributesItemsSource", typeof(IEnumerable), typeof(ManagePlantAttributesListControl), new PropertyMetadata(null));


        public ICommand RemoveElementCommand
        {
            get { return (ICommand)GetValue(RemoveElementCommandProperty); }
            set { SetValue(RemoveElementCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveElementCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveElementCommandProperty =
            DependencyProperty.Register("RemoveElementCommand", typeof(ICommand), typeof(ManagePlantAttributesListControl), new PropertyMetadata(null));

    }

}
