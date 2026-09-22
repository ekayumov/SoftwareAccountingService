using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoftwareAccountingService.Wpf.Infrastructure.Api;
using SoftwareAccountingService.Wpf.Presentation.Models;
using SoftwareAccountingService.Wpf.Presentation.Models.Interfaces;
using SoftwareAccountingService.Wpf.Presentation.Navigation;
using System.Net.Http;

namespace SoftwareAccountingService.Wpf.Presentation.ViewModels
{
    public partial class InspectionObjectFormViewModel :
        ObservableObject,
        INavigationAware
    {
        private readonly IInspectionObjectsApiClient _apiClient;
        private readonly INavigationService _navigationService;

        private Guid? _inspectionObjectId;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCreateMode))]
        [NotifyPropertyChangedFor(nameof(PageTitle))]
        [NotifyPropertyChangedFor(nameof(SaveButtonText))]
        private bool isEditMode;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string version = string.Empty;

        [ObservableProperty]
        private DateTime? receivedDate;

        [ObservableProperty]
        private string? note;

        [ObservableProperty]
        private List<FilterOptionModel> typeOptions = new();

        [ObservableProperty]
        private List<FilterOptionModel> resultOptions = new();

        [ObservableProperty]
        private FilterOptionModel? selectedType;

        [ObservableProperty]
        private FilterOptionModel? selectedResult;

        [ObservableProperty]
        private string? errorMessage;

        public bool IsCreateMode => !IsEditMode;

        public string PageTitle =>
            IsEditMode
                ? "Редактирование объекта"
                : "Новый объект";

        public string SaveButtonText =>
            IsEditMode ? "Сохранить" : "Создать";

        public InspectionObjectFormViewModel(
            IInspectionObjectsApiClient apiClient,
            INavigationService navigationService)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
        }

        public async Task OnNavigatedToAsync(object? parameter)
        {
            ErrorMessage = null;
            IsEditMode = parameter is Guid;
            _inspectionObjectId =
                parameter is Guid id ? id : null;

            try
            {
                await LoadOptionsAsync();

                if (_inspectionObjectId is Guid _id)
                {
                    await LoadObjectAsync(_id);
                }
                else
                {
                    SelectedResult = ResultOptions.FirstOrDefault(
                        option => option.Code == "InProgress");
                }
            }
            catch (Exception exception)
            {
                ErrorMessage = GetErrorMessage(exception);
            }
        }

        [RelayCommand]
        private Task CancelAsync()
        {
            return _navigationService.NavigateToListAsync();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            ErrorMessage = ValidateForm();

            if (ErrorMessage is not null)
            {
                return;
            }

            try
            {
                if (IsEditMode)
                {
                    await UpdateAsync();
                }
                else
                {
                    await CreateAsync();
                }

                await _navigationService.NavigateToListAsync();
            }
            catch (Exception exception)
            {
                ErrorMessage = GetErrorMessage(exception);
            }
        }

        private async Task LoadOptionsAsync()
        {
            InspectionFilterOptionsModel options =
                await _apiClient.GetFilterOptionsAsync(
                    CancellationToken.None);

            TypeOptions = options.Types;
            ResultOptions = options.Results;
        }

        private async Task LoadObjectAsync(Guid id)
        {
            InspectionObjectModel inspectionObject =
                await _apiClient.GetByIdAsync(
                    id,
                    CancellationToken.None);

            Name = inspectionObject.Name;
            Version = inspectionObject.Version;
            ReceivedDate = inspectionObject.ReceivedDate;
            Note = inspectionObject.Note;

            SelectedType = TypeOptions.FirstOrDefault(
                option => option.Code == inspectionObject.Type);

            SelectedResult = ResultOptions.FirstOrDefault(
                option => option.Code == inspectionObject.Result);
        }

        private Task CreateAsync()
        {
            var request = new CreateInspectionObjectRequest
            {
                Name = Name.Trim(),
                Version = Version.Trim(),
                Type = SelectedType!.Code,
                ReceivedDate = ReceivedDate!.Value,
                Note = NormalizeNote()
            };

            return _apiClient.CreateAsync(
                request,
                CancellationToken.None);
        }

        private Task UpdateAsync()
        {
            var request = new UpdateInspectionResultRequest
            {
                Result = SelectedResult!.Code,
                Note = NormalizeNote()
            };

            return _apiClient.UpdateResultAsync(
                _inspectionObjectId!.Value,
                request,
                CancellationToken.None);
        }

        private string? ValidateForm()
        {
            if (Note?.Length > 1000)
            {
                return "Примечание не должно превышать 1000 символов.";
            }

            if (IsEditMode)
            {
                return SelectedResult is null
                    ? "Выберите результат проверки."
                    : null;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                return "Введите наименование.";
            }

            if (Name.Trim().Length > 200)
            {
                return "Наименование не должно превышать 200 символов.";
            }

            if (string.IsNullOrWhiteSpace(Version))
            {
                return "Введите версию.";
            }

            if (Version.Trim().Length > 50)
            {
                return "Версия не должна превышать 50 символов.";
            }

            if (SelectedType is null)
            {
                return "Выберите тип объекта.";
            }

            return ReceivedDate is null
                ? "Выберите дату получения."
                : null;
        }

        private string? NormalizeNote()
        {
            return string.IsNullOrWhiteSpace(Note)
                ? null
                : Note.Trim();
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
