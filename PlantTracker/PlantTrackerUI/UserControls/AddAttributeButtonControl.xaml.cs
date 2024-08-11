using System;
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
    /// Interaction logic for AddAttributeButtonControl.xaml
    /// </summary>
    public partial class AddAttributeButtonControl : UserControl
    {
        public AddAttributeButtonControl()
        {
            InitializeComponent();
        }

        public string AddButtonContent
        {
            get { return (string)GetValue(AddButtonContentProperty); }
            set { SetValue(AddButtonContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for AddButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonContentProperty =
            DependencyProperty.Register("AddButtonContent", typeof(string), typeof(AddAttributeButtonControl), new PropertyMetadata("Add..."));



        public ICommand AddElementWindowCommand
        {
            get { return (ICommand)GetValue(AddElementWindowCommandProperty); }
            set { SetValue(AddElementWindowCommandProperty, value); }
        }
        public static readonly DependencyProperty AddElementWindowCommandProperty =
            DependencyProperty.Register("AddElementWindowCommand", typeof(ICommand), typeof(AddAttributeButtonControl), new PropertyMetadata(null));

    }
}
