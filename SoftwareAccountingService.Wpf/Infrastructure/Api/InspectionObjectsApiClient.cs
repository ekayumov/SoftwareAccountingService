using SoftwareAccountingService.Wpf.Presentation.Models;
using SoftwareAccountingService.Wpf.Presentation.Models.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace SoftwareAccountingService.Wpf.Infrastructure.Api
{
    public sealed class InspectionObjectsApiClient :
        IInspectionObjectsApiClient
    {
        private const string BaseRoute =
            "api/inspection-objects";

        private readonly HttpClient _httpClient;

        public InspectionObjectsApiClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<InspectionObjectModel>>
            GetAllAsync(
                string? search,
                string? type,
                string? result,
                CancellationToken cancellationToken)
        {
            var queryParameters = new List<string>();

            AddQueryParameter(
                queryParameters,
                "search",
                search);

            AddQueryParameter(
                queryParameters,
                "type",
                type);

            AddQueryParameter(
                queryParameters,
                "result",
                result);

            string requestUri = BaseRoute;

            if (queryParameters.Count > 0)
            {
                requestUri +=
                    "?" + string.Join("&", queryParameters);
            }

            using HttpResponseMessage response =
                await _httpClient.GetAsync(
                    requestUri,
                    cancellationToken);

            List<InspectionObjectModel> objects =
                await ReadResponseAsync<
                    List<InspectionObjectModel>>(
                        response,
                        cancellationToken);

            return objects;
        }

        public async Task<InspectionObjectModel> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            using HttpResponseMessage response =
                await _httpClient.GetAsync(
                    $"{BaseRoute}/{id}",
                    cancellationToken);

            return await ReadResponseAsync<InspectionObjectModel>(
                response,
                cancellationToken);
        }

        public async Task<InspectionObjectModel> CreateAsync(
            CreateInspectionObjectRequest request,
            CancellationToken cancellationToken)
        {
            using HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(
                    BaseRoute,
                    request,
                    cancellationToken);

            return await ReadResponseAsync<InspectionObjectModel>(
                response,
                cancellationToken);
        }

        public async Task<InspectionObjectModel> UpdateResultAsync(
            Guid id,
            UpdateInspectionResultRequest request,
            CancellationToken cancellationToken)
        {
            using HttpResponseMessage response =
                await _httpClient.PatchAsJsonAsync(
                    $"{BaseRoute}/{id}/result",
                    request,
                    cancellationToken);

            return await ReadResponseAsync<InspectionObjectModel>(
                response,
                cancellationToken);
        }

        public async Task<InspectionFilterOptionsModel>
            GetFilterOptionsAsync(
                CancellationToken cancellationToken)
        {
            using HttpResponseMessage response =
                await _httpClient.GetAsync(
                    $"{BaseRoute}/filter-options",
                    cancellationToken);

            return await ReadResponseAsync<
                InspectionFilterOptionsModel>(
                    response,
                    cancellationToken);
        }

        private static void AddQueryParameter(
            List<string> queryParameters,
            string name,
            string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            string encodedValue =
                Uri.EscapeDataString(value.Trim());

            queryParameters.Add(
                $"{name}={encodedValue}");
        }

        private static async Task<T> ReadResponseAsync<T>(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                await ThrowApiExceptionAsync(
                    response,
                    cancellationToken);
            }

            T? result =
                await response.Content.ReadFromJsonAsync<T>(
                    cancellationToken);

            if (result is null)
            {
                throw new InvalidOperationException(
                    "API вернуло пустой ответ.");
            }

            return result;
        }

        private static async Task ThrowApiExceptionAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            string responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            string message = response.StatusCode switch
            {
                HttpStatusCode.BadRequest =>
                    "Сервер отклонил введённые данные.",

                HttpStatusCode.NotFound =>
                    "Объект проверки не найден.",

                HttpStatusCode.InternalServerError =>
                    "На сервере произошла внутренняя ошибка.",

                _ =>
                    $"API вернуло ошибку " +
                    $"{(int)response.StatusCode}."
            };

            throw new ApiException(
                response.StatusCode,
                message,
                responseBody);
        }
    }
}