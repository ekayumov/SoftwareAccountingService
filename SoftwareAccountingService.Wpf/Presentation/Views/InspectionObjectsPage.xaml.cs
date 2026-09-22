using SoftwareAccountingService.Wpf.Presentation.ViewModels;
using System.Windows.Controls;

namespace SoftwareAccountingService.Wpf.Presentation.Views
{
    public partial class InspectionObjectsPage : Page
    {
        public InspectionObjectsPage(
            InspectionObjectsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
