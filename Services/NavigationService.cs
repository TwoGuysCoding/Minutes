﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;

namespace Minutes.Services
{
    public partial class NavigationService(Func<Type, ViewModel> viewModelFactory) : ObservableObject, INavigationService
    {

        [ObservableProperty]
        private ViewModel? _currentView;


        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            var viewModel = viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}