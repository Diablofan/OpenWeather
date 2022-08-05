using Microsoft.Extensions.Options;
using OpenWeather.Exceptions;
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
        Task<LocationResult> GetCoordinatesByLocationName(string cityState, string countryCode = "US");

        Task<CurrentConditionResults> GetCurrentConditionsByCoordinate(Coordinate coordinates);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _apiClient;
        private readonly IOptions<OpenWeatherSettings> _weatherSettings;
        private readonly ILogger<IWeatherService> _logger;

        public WeatherService(HttpClient apiClient, IOptions<OpenWeatherSettings> weatherSettings, ILogger<IWeatherService> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _weatherSettings = weatherSettings ?? throw new ArgumentNullException(nameof(weatherSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _apiClient.BaseAddress = new Uri(weatherSettings.Value.Endpoint);
        }

        public async Task<LocationResult> GetCoordinatesByLocationName(string cityState, string countryCode)
        {
            var route = $"/geo/1.0/direct?limit=1&q={cityState},{countryCode}";
            var results = await SendRequest<List<LocationNameResult>>(route);

            if (results.Count == 0)
            {
                throw new OpenWeatherException($"No Locations Found for: {cityState} in {countryCode}");
            }

            return results.First();
        }

        public async Task<LocationResult> GetCoordinatesByPostalCode(string postalCode, string countryCode)
        {
            var route = $"/geo/1.0/zip?zip={postalCode},{countryCode}";
            return await SendRequest<PostalCodeResult>(route);
        }

        public async Task<CurrentConditionResults> GetCurrentConditionsByCoordinate(Coordinate coordinates)
        {
            var route = $"/data/2.5/weather?lat={coordinates.Latitude}&lon={coordinates.Longitude}";
            return await SendRequest<CurrentConditionResults>(route);
        }

        private async Task<T> SendRequest<T>(string route)
        {
            var result = await _apiClient.GetAsync($"{route}&appid={_weatherSettings.Value.ApiKey}");

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await HandleOpenWeatherException(result);
            }

            return await ReadResult<T>(result);
        }

        private async Task HandleOpenWeatherException(HttpResponseMessage message)
        {
            var error = await ReadResult<OpenWeatherError>(message);

            _logger.LogError($"Error from OpenWeatherMap: Code={error.ErrorCode} | Message={error.Message}");

            var ex = new OpenWeatherException(error.Message);
            ex.Data.Add("Code", error.ErrorCode);
            throw ex;
        }

        private static async Task<T> ReadResult<T>(HttpResponseMessage message)
        {
            var stream = await message.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<T>(stream);

            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return result;
        }
    }
}
