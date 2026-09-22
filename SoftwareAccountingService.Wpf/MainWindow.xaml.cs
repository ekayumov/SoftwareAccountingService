using SoftwareAccountingService.Wpf.Presentation.Navigation;
using System.Windows;

namespace SoftwareAccountingService.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow(
            NavigationService navigationService)
        {
            InitializeComponent();

            navigationService.Initialize(MainFrame);

            Loaded += async (_, _) =>
                await navigationService.NavigateToListAsync();
        }
    }
}
