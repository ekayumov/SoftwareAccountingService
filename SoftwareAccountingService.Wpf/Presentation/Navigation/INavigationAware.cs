namespace SoftwareAccountingService.Wpf.Presentation.Navigation
{
    public interface INavigationAware
    {
        Task OnNavigatedToAsync(object? parameter);
    }
}
