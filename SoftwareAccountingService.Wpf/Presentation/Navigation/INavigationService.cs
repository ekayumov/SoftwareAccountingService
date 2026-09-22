using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareAccountingService.Wpf.Presentation.Navigation
{
    public interface INavigationService
    {
        Task NavigateToListAsync();

        Task NavigateToFormAsync( Guid? inspectionObjectId = null);
    }
}
