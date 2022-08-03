using OpenWeather.Models;
using OpenWeather.Pages;

namespace OpenWeather
{
    public static class Extensions
    {
        public static Coordinate ToConditionCoordinate(this Lookup lookup)
            => new() {
                Longitude = lookup.Longitude,
                Latitude = lookup.Latitude
            };
    }
}
