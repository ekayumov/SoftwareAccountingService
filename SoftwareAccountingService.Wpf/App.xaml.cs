using Microsoft.Extensions.DependencyInjection;
using SoftwareAccountingService.Wpf.Presentation.ViewModels;
using SoftwareAccountingService.Wpf.Presentation.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SoftwareAccountingService.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            services.AddTransient<InspectionObjectsPage>();
            services.AddTransient<InspectionObjectFormPage>();

            services.AddTransient<InspectionObjectsViewModel>();
            services.AddTransient<InspectionObjectFormViewModel>();

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

        }

        protected override void OnExit(
            ExitEventArgs e)
        {
            _serviceProvider?.Dispose();

            base.OnExit(e);
        }
    }

}
