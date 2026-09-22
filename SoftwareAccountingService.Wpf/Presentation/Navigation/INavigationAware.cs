using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareAccountingService.Wpf.Presentation.Navigation
{
    public interface INavigationAware
    {
        Task OnNavigatedToAsync(object? parameter);
    }
}
