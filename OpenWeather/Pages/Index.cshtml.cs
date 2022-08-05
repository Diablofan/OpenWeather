using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace OpenWeather.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public Dictionary<string, string> CountryCodes { get; }

        public IndexModel(ILogger<IndexModel> logger, IOptions<Dictionary<string, string>> countryCodes)
        {
            _logger = logger;
            CountryCodes = countryCodes.Value;
        }

        public void OnGet() { }
    }
}