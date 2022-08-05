using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenWeather.Models;
using OpenWeather.Services;
using System.Text.RegularExpressions;

namespace OpenWeather.Pages
{
    public class WeatherModel : PageModel
    {
        private readonly IWeatherService weatherService;

        public WeatherModel(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        // Disallow direct navigation
        public void OnGet() => Response.Redirect("/");

        public async Task OnPost(Lookup lookup)
        {
            Coordinate coordinate;

            if(string.IsNullOrEmpty(lookup.NameOrPostCode))
            {
                coordinate = new() {
                    Longitude = lookup.Longitude,
                    Latitude = lookup.Latitude
                };
            }
            else if(lookup.IsPostCode)
            {
                coordinate = await weatherService.GetCoordinatesByPostalCode(lookup.NameOrPostCode, lookup.Country);
            }
            else
            {
                coordinate = await weatherService.GetCoordinatesByLocationName(lookup.NameOrPostCode, lookup.Country);
            }

            CurrentCondition = await weatherService.GetCurrentConditionsByCoordinate(coordinate);
        }

        public CurrentConditionResults? CurrentCondition { get; set; }
    }

    public class Lookup
    {
        [BindProperty(Name = "nameOrPostCodeLookup")]
        public string NameOrPostCode { get; set; } = string.Empty;

        [BindProperty(Name = "country")]
        public string Country { get; set; } = string.Empty;

        [BindProperty(Name = "lat")]
        public double Latitude { get; set; }

        [BindProperty(Name = "lon")]
        public double Longitude { get; set; }

        public bool IsPostCode => Regex.IsMatch(NameOrPostCode, @"^([0-9]{5})(-[0-9]{4})?$");
    }
}
