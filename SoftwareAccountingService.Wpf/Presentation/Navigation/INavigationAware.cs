using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareAccountingService.Wpf.Presentation.Navigation
{
    internal interface INavigationAware
    {
        Task OnNavigatedToAsync(object? parameter);
    }
}
