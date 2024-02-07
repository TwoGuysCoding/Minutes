using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minutes.Core;

namespace Minutes.Services
{
    internal interface INavigationService
    {
        ViewModel CurrentView { get; }

        void NavigateTo<TViewModel>() where TViewModel : ViewModel;
    }
}
