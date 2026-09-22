using Microsoft.Extensions.DependencyInjection;
using SoftwareAccountingService.Wpf.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SoftwareAccountingService.Wpf.Presentation.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider; //провайдер
        private Frame? _frame; //фрейм для навигации

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize(Frame frame)
        {
            _frame = frame;
        }

        public Task NavigateToListAsync()
        {
            return NavigateAsync<InspectionObjectsPage>();
        }

        public Task NavigateToFormAsync(
            Guid? inspectionObjectId = null)
        {
            return NavigateAsync<InspectionObjectFormPage>(
                inspectionObjectId);
        }

        private async Task NavigateAsync<TPage>(object? parameter = null) where TPage : Page
        {
            if (_frame == null) { throw new InvalidOperationException("NavigationService is not initialized. Call Initialize() first."); }
          
            TPage page = _serviceProvider.GetRequiredService<TPage>();
            _frame.Navigate(page);

            if (page.DataContext is INavigationAware navigationAware)
            {
                await navigationAware.OnNavigatedToAsync(
                    parameter);
            }
        }

    }
}
