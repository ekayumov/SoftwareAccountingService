using System.Net;

namespace SoftwareAccountingService.Wpf.Infrastructure.Api
{
    public sealed class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string ResponseBody { get; }

        public ApiException(
            HttpStatusCode statusCode,
            string message,
            string responseBody)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}