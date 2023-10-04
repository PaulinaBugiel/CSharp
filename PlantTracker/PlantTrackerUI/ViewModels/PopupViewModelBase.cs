using PlantTrackerUI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.ViewModels
{
    /// <summary>
    /// Class providing a closing command which executes CloseAction if it was set in the view model
    /// </summary>
    public class PopupViewModelBase
    {
        public Action? CloseAction { private get; set; }

        public void CloseWindow()
        {
            if (CloseAction != null)
                CloseAction();
        }
    }
}
