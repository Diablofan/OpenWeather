using System.Runtime.Serialization;

namespace OpenWeather.Exceptions
{
    /// <summary>
    /// Represents an exception thrown by the OpenWeather API.
    /// The message returned by the API will be the exception message,
    /// and the Error code will be handled as a "Code" entry in the data
    /// property.
    /// </summary>
    [Serializable]
    public class OpenWeatherException : Exception
    {
        public OpenWeatherException()
        {
        }

        public OpenWeatherException(string? message) : base(message)
        {
        }

        public OpenWeatherException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OpenWeatherException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
