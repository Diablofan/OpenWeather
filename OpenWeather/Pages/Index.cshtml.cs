using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using OpenWeather.Models;

namespace OpenWeather.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public Dictionary<string, string> CountryCodes { get; }
        public IOptions<OpenWeatherSettings> OwSettings { get; }

        public IndexModel(ILogger<IndexModel> logger, IOptions<Dictionary<string, string>> countryCodes, IOptions<OpenWeatherSettings> owSettings)
        {
            _logger = logger;
            OwSettings = owSettings;
            CountryCodes = countryCodes.Value;
        }

        public void OnGet() { }
    }
}