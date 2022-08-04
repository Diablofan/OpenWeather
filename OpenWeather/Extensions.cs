using OpenWeather.Models;
using OpenWeather.Pages;

namespace OpenWeather
{
    public static class Extensions
    {
        private static readonly DateTime unixTimeStampBase = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a UTC Unix Timestamp to a UTC Date Time object.
        /// </summary>
        /// <param name="timestamp">The number of seconds since 1970-01-01 00:00:00 UTC</param>
        /// <returns>A date time representing the time specified</returns>
        public static DateTime FromUnixTimeStamp(this long timestamp)
        {
            return unixTimeStampBase.AddSeconds(timestamp);
        }

        /// <summary>
        /// Converts a kelvin based temperature to fahrenheit
        /// </summary>
        /// <param name="kelvin">The temperature in kelvin</param>
        /// <returns>The temperature in fahrenheit</returns>
        public static double ToFahrenheit(this double kelvin)
        {
            static double ToCelcius(double k) => k - 273.15;

            return ToCelcius(kelvin) * 1.8 + 32;
        }
    }
}
