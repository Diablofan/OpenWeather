using System.Text.Json.Serialization;

namespace OpenWeather.Models
{
    public record Coordinate
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
    }

    public abstract record LocationResult : Coordinate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }

    public record PostalCodeResult : LocationResult
    {
        [JsonPropertyName("zip")]
        public string PostCode { get; set; } = string.Empty;
    }

    public record LocationNameResult : LocationResult
    {

        [JsonPropertyName("local_names")]
        public Dictionary<string, string> LocalNames { get; set; } = new();

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
    }
}
