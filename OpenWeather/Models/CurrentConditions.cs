using System.Text.Json.Serialization;

namespace OpenWeather.Models
{
    public record ConditionCodes
    {
        [JsonPropertyName("id")]
        public long ConditionId { get; set; }

        [JsonPropertyName("main")]
        public string OneWordDescription { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string LongDescription { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string IconId { get; set; } = string.Empty;
    }

    public record AirTemperatures
    {
        [JsonPropertyName("temp")]
        public double ActualTempKelvin { get;set;}

        [JsonPropertyName("feels_like")]
        public double HumanTempKelvin { get;set;}

        [JsonPropertyName("temp_min")]
        public double MinimumTemperature { get;set;}

        [JsonPropertyName("temp_max")]
        public double MaximumTemperature { get;set;}

        [JsonPropertyName("pressure")]
        public double AirPressure { get;set;}

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("sea_level")]
        public double AirPressureAtSeaLevel { get; set; }


        [JsonPropertyName("grnd_level")]
        public double AirPressureAtGroundLevel { get; set; }
    }

    public record Wind
    {
        [JsonPropertyName("speed")]
        public double Speed{ get; set; }

        [JsonPropertyName("deg")]
        public int DegreesFromNorth { get; set; }

        [JsonPropertyName("gust")]
        public double GustsMeterPerSecond { get; set; }
    }

    public record Cloudiness
    {
        [JsonPropertyName("all")]
        public int CloudCover { get; set; }
    }

    public record Percipitation
    {
        [JsonPropertyName("1h")]
        public double OneHour { get; set; }

        [JsonPropertyName("3h")]
        public double ThreeHour { get; set; }
    }

    public record System
    {
        // (int type, long id, string country, long sunset, long sunrise);
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("id")]
        public long Id{ get; set; }


        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;


        [JsonPropertyName("sunrise")]
        public long SunriseUTC { get; set; }


        [JsonPropertyName("sunset")]
        public long SunsetUTC { get; set; }
    }

    public record CurrentConditionResults
    {
        [JsonPropertyName("coord")]
        public Coordinate Coordinate { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<ConditionCodes> Weather { get; set; } = new();

        [JsonPropertyName("base")]
        public string Base { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public AirTemperatures AirTemperatures { get; set; } = new();

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = new Wind();

        [JsonPropertyName("clouds")]
        public Cloudiness Clouds { get; set; } = new();

        [JsonPropertyName("rain")]
        public Percipitation Rain { get; set; } = new();

        [JsonPropertyName("snow")]
        public Percipitation Snow { get; set; } = new();

        [JsonPropertyName("dt")]
        public long ReportTimestampUTC { get; set; }

        [JsonPropertyName("sys")]
        public System Sys { get; set; } = new();

        [JsonPropertyName("timezone")]
        public int TimezoneOffset { get; set; }

        [JsonPropertyName("id")]
        public long CityId { get; set; }

        [JsonPropertyName("name")]
        public string CityName { get; set; } = string.Empty;

        [JsonPropertyName("cod")]
        public long InternalParam { get; set; }

        [JsonPropertyName("visibility")]
        public int VisibilityInMeters { get; set; }
    }
}
