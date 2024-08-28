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

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id")
            {
                //e.Cancel = true;
                e.Column.IsReadOnly = true;
                Style style = new Style();
                style.Setters.Add(new Setter
                {
                    Property = BackgroundProperty,
                    Value = new SolidColorBrush(Color.FromRgb(0xEE, 0xEE, 0xEE))
                });
                style.Setters.Add(new Setter
                {
                    Property = ForegroundProperty,
                    Value = Brushes.Gray
                });


                e.Column.CellStyle = style;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta/10.0);
            e.Handled = true;
        }
    }
}
