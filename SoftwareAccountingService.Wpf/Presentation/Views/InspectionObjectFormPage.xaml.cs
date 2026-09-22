using SoftwareAccountingService.Wpf.Presentation.ViewModels;
using System.Windows.Controls;

namespace SoftwareAccountingService.Wpf.Presentation.Views
{
    public partial class InspectionObjectFormPage : Page
    {
        public InspectionObjectFormPage(
            InspectionObjectFormViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
