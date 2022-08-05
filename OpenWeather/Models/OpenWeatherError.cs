using System.Text.Json.Serialization;

namespace OpenWeather.Models
{
    public class OpenWeatherError
    {
        [JsonPropertyName("cod")]
        public string ErrorCode { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
