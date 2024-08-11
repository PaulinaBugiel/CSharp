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
        public ManagePlantAttributesView()
        {
            InitializeComponent();
        }

        //public ICommand RemoveAttributeViewCommand
        //{
        //    get { return (ICommand)GetValue(RemoveAttributeViewCommandProperty); }
        //    set { SetValue(RemoveAttributeViewCommandProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for RemoveAttributeViewCommand.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty RemoveAttributeViewCommandProperty =
        //    DependencyProperty.Register("RemoveAttributeViewCommand", typeof(ICommand), typeof(ManagePlantAttributesView), new PropertyMetadata(null));
    }
}
