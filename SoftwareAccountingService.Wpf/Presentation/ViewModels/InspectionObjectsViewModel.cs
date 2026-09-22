using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoftwareAccountingService.Wpf.Presentation.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareAccountingService.Wpf.Presentation.ViewModels
{
    public class InspectionObjectsViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        public InspectionObjectsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Task OnNavigatedToAsync(object? parameter)
        {
            // Позже здесь загрузим список объектов через API.
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task OpenCreateFormAsync()
        {
            return _navigationService.NavigateToFormAsync();
        }

        [RelayCommand]
        private Task OpenEditFormAsync(
            Guid inspectionObjectId)
        {
            return _navigationService.NavigateToFormAsync(
                inspectionObjectId);
        }
    }
}
