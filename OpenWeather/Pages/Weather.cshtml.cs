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

            if(string.IsNullOrEmpty(lookup.CityStateZip))
            {
                coordinate = new() {
                    Longitude = lookup.Longitude,
                    Latitude = lookup.Latitude
                };
            }
            else if(lookup.IsZipCode)
            {
                coordinate = await weatherService.GetCoordinatesByPostalCode(lookup.Zip);
            }
            else
            {
                coordinate = await weatherService.GetCoordinateByCityState(lookup.City, lookup.State);
            }

            CurrentCondition = await weatherService.GetCurrentConditionsByCoordinate(coordinate);
        }

        public CurrentConditionResults CurrentCondition { get; set; }
    }

    public class Lookup
    {
        [BindProperty(Name = "cszLookup")]
        public string CityStateZip { get; set; } = string.Empty;

        [BindProperty(Name = "lat")]
        public double Latitude { get; set; }

        [BindProperty(Name = "lon")]
        public double Longitude { get; set; }

        public bool IsZipCode => Regex.IsMatch(CityStateZip, @"^([0-9]{5})(-[0-9]{4})?$");

        public string City => CityStateZip.Split(",")[0].Trim() ?? string.Empty;

        public string State
        {
            get
            {
                if (string.IsNullOrEmpty(CityStateZip) || IsZipCode || !CityStateZip.Contains(","))
                {
                    return string.Empty;
                }

                return CityStateZip.Split(",")?[1]?.Trim() ?? string.Empty;
            }
        }

        public string Zip => CityStateZip;
    }
}
