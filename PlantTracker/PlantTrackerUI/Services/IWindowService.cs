﻿using PlantTrackerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Services
{
    public interface IWindowService
    {
        void ShowWindow(PopupViewModelBase dataContext);
        void CloseWindow();
    }
}
