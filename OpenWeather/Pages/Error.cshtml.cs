using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace OpenWeather.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ExceptionMessage { get; private set; } = string.Empty;

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet() => Handle();

        public void OnPost() => Handle();

        private void Handle()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ExceptionMessage = "Something Happened.";

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if(exceptionHandlerPathFeature is not null)
            {
                Exception ex = exceptionHandlerPathFeature.Error;
                _logger.LogError(ex, ex.Message, Array.Empty<object>());
            }
        }
    }
}
