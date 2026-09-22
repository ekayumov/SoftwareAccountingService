using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoftwareAccountingService.Wpf.Presentation.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareAccountingService.Wpf.Presentation.ViewModels
{
    public class InspectionObjectFormViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private Guid? inspectionObjectId;

        [ObservableProperty]
        private bool isEditMode;

        [ObservableProperty]
        private string pageTitle = "Новый объект";
        public InspectionObjectFormViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public Task OnNavigatedToAsync(object? parameter)
        {
            if (parameter is Guid id)
            {
                InspectionObjectId = id;
                IsEditMode = true;
                PageTitle = "Редактирование объекта";
            }
            else
            {
                InspectionObjectId = null;
                IsEditMode = false;
                PageTitle = "Новый объект";
            }

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task CancelAsync()
        {
            return _navigationService.NavigateToListAsync();
        }
    }
 }
