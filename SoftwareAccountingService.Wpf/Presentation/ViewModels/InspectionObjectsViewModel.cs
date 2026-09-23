using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoftwareAccountingService.Wpf.Infrastructure.Api;
using SoftwareAccountingService.Wpf.Presentation.Models;
using SoftwareAccountingService.Wpf.Presentation.Models.Interfaces;
using SoftwareAccountingService.Wpf.Presentation.Navigation;
using System.Net.Http;

namespace SoftwareAccountingService.Wpf.Presentation.ViewModels
{
    public partial class InspectionObjectsViewModel :
        ObservableObject,
        INavigationAware
    {
        private readonly IInspectionObjectsApiClient _apiClient;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private List<InspectionObjectModel> inspectionObjects = new();

        [ObservableProperty]
        private List<FilterOptionModel> typeOptions = new();

        [ObservableProperty]
        private List<FilterOptionModel> resultOptions = new();

        [ObservableProperty]
        private string? searchText;

        [ObservableProperty]
        private FilterOptionModel? selectedType;

        [ObservableProperty]
        private FilterOptionModel? selectedResult;

        [ObservableProperty]
        private string? errorMessage;

        public InspectionObjectsViewModel(
            IInspectionObjectsApiClient apiClient,
            INavigationService navigationService)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
        }

        public Task OnNavigatedToAsync(object? parameter)
        {
            return LoadDataAsync(loadFilters: true);
        }

        [RelayCommand]
        private Task SearchAsync()
        {
            return LoadDataAsync(
                loadFilters: TypeOptions.Count == 0);
        }

        [RelayCommand]
        private Task OpenCreateFormAsync()
        {
            return _navigationService.NavigateToFormAsync();
        }

        [RelayCommand]
        private Task OpenEditFormAsync(Guid id)
        {
            return _navigationService.NavigateToFormAsync(id);
        }

        private async Task LoadDataAsync(bool loadFilters)
        {
            ErrorMessage = null;

            try
            {
                if (loadFilters)
                {
                    await LoadFiltersAsync();
                }

                await LoadObjectsAsync();
            }
            catch (Exception exception)
            {
                ErrorMessage = GetErrorMessage(exception);
            }
        }

        private async Task LoadFiltersAsync()
        {
            InspectionFilterOptionsModel filters =
                await _apiClient.GetFilterOptionsAsync(
                    CancellationToken.None);

            TypeOptions = new List<FilterOptionModel>
            {
                new()
                {
                    Code = string.Empty,
                    DisplayName = "Все типы"
                }
            };

            TypeOptions.AddRange(filters.Types);
            SelectedType = TypeOptions[0];

            ResultOptions = new List<FilterOptionModel>
            {
                new()
                {
                    Code = string.Empty,
                    DisplayName = "Все результаты"
                }
            };

            ResultOptions.AddRange(filters.Results);
            SelectedResult = ResultOptions[0];
        }

        private async Task LoadObjectsAsync()
        {
            IReadOnlyList<InspectionObjectModel> objects =
                await _apiClient.GetAllAsync(
                    SearchText,
                    SelectedType?.Code,
                    SelectedResult?.Code,
                    CancellationToken.None);

            foreach (InspectionObjectModel inspectionObject in objects) // плохое решение, но думаю допустимое
            {
                inspectionObject.TypeDisplayName =
                    TypeOptions
                        .FirstOrDefault(option =>
                            option.Code == inspectionObject.Type)
                        ?.DisplayName
                    ?? inspectionObject.Type;

                inspectionObject.ResultDisplayName =
                    ResultOptions
                        .FirstOrDefault(option =>
                            option.Code == inspectionObject.Result)
                        ?.DisplayName
                    ?? inspectionObject.Result;
            }

            InspectionObjects = objects.ToList();
        }

        private static string GetErrorMessage(
            Exception exception)
        {
            return exception switch
            {
                ApiException apiException => apiException.Message,
                HttpRequestException =>
                    "Не удалось подключиться к API.",
                TaskCanceledException =>
                    "Сервер слишком долго не отвечает.",
                _ => exception.Message
            };
        }
    }
}
