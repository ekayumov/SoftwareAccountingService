using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftwareAccountingService.Wpf.Infrastructure.Api;
using SoftwareAccountingService.Wpf.Presentation.Models.Interfaces;
using SoftwareAccountingService.Wpf.Presentation.Navigation;
using SoftwareAccountingService.Wpf.Presentation.ViewModels;
using SoftwareAccountingService.Wpf.Presentation.Views;
using System.Windows;

namespace SoftwareAccountingService.Wpf
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(
            StartupEventArgs e)
        {
            base.OnStartup(e);

            IConfiguration configuration =
                new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: false)
                    .Build();

            string apiBaseUrl =
                configuration["Api:BaseUrl"]
                ?? throw new InvalidOperationException(
                    "Адрес API не найден в appsettings.json.");

            apiBaseUrl =
                apiBaseUrl.TrimEnd('/') + "/";

            var services = new ServiceCollection();

            services.AddSingleton<NavigationService>();
            services.AddSingleton<INavigationService>(
                serviceProvider =>
                    serviceProvider
                        .GetRequiredService<NavigationService>());

            services.AddHttpClient<
                IInspectionObjectsApiClient,
                InspectionObjectsApiClient>(
                    httpClient =>
                    {
                        httpClient.BaseAddress =
                            new Uri(apiBaseUrl);

                        httpClient.Timeout =
                            TimeSpan.FromSeconds(30);
                    });

            services.AddSingleton<MainWindow>();
            services.AddTransient<InspectionObjectsPage>();
            services.AddTransient<InspectionObjectFormPage>();
            services.AddTransient<InspectionObjectsViewModel>();
            services.AddTransient<InspectionObjectFormViewModel>();

            _serviceProvider =
                services.BuildServiceProvider();

            MainWindow mainWindow =
                _serviceProvider
                    .GetRequiredService<MainWindow>();

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
