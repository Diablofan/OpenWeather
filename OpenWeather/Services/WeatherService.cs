using Microsoft.Extensions.Options;
using OpenWeather.Models;
using System.Text.Json;

namespace OpenWeather.Services
{
    public interface IWeatherService
    {
        /// <summary>
        /// Retrieves from OpenWeatherMap coordinates, city name and country based on the provided postal code and country
        /// </summary>
        /// <param name="postalCode">The postal code to look up by</param>
        /// <param name="countryCode">The 2 character ISO 3166 country code the postal code belongs to</param>
        /// <returns>A set of coordinates for representing the postal code country combination</returns>
        Task<LocationResult> GetCoordinatesByPostalCode(string postalCode, string countryCode = "US");

        /// <summary>
        /// Retrieves from OpenWeatherMap coordinates, city name and country based on the provided 
        /// </summary>
        /// <param name="city">The city to look up coordinates for</param>
        /// <param name="state">The state of the city, may be blank</param>
        /// <param name="countryCode">The 2 character ISO 3166 country code the postal code belongs to</param>
        /// <returns>A set of coordinates for representing the postal code country combination</returns>
        Task<LocationResult> GetCoordinateByCityState(string city, string state, string countryCode = "US");

        Task<CurrentConditionResults> GetCurrentConditionsByCoordinate(Coordinate coordinates);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient apiClient;
        private readonly IOptions<OpenWeatherSettings> weatherSettings;

        public WeatherService(HttpClient apiClient, IOptions<OpenWeatherSettings> weatherSettings)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.weatherSettings = weatherSettings ?? throw new ArgumentNullException(nameof(weatherSettings));

            apiClient.BaseAddress = new Uri(weatherSettings.Value.Endpoint);
        }

        public async Task<LocationResult> GetCoordinateByCityState(string city, string state, string countryCode)
        {
            var route = $"/geo/1.0/direct?appid={weatherSettings.Value.ApiKey}&limit=1&q={city},{state},{countryCode}";
            var result = await apiClient.GetStringAsync(route);

            if(result == null)
            {
                // TODO: Proper exception...
                throw new ArgumentNullException(nameof(result));
            }

            var location = JsonSerializer.Deserialize<List<LocationNameResult>>(result);

            return location.First();
        }

        public async Task<LocationResult> GetCoordinatesByPostalCode(string postalCode, string countryCode)
        {
            var route = $"/geo/1.0/zip?appid={weatherSettings.Value.ApiKey}&zip={postalCode},{countryCode}";
            var result = await apiClient.GetStringAsync(route);

            if(result == null)
            {
                // TODO: Proper exception...
                throw new ArgumentNullException(nameof(result));
            }

            var coordinate = JsonSerializer.Deserialize<PostalCodeResult>(result);

            return coordinate;
        }

        public async Task<CurrentConditionResults> GetCurrentConditionsByCoordinate(Coordinate coordinates)
        {
            var route = $"/data/2.5/weather?appid={weatherSettings.Value.ApiKey}&lat={coordinates.Latitude}&lon={coordinates.Longitude}";
            var result = await apiClient.GetStringAsync(route);

            if(result == null)
            {
                // TODO: Proper exception...
                throw new ArgumentNullException(nameof(result));
            }

            var conditions = JsonSerializer.Deserialize<CurrentConditionResults>(result);

            return conditions;
        }
    }
}
