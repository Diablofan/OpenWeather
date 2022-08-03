namespace OpenWeather.Tests
{
    public class WeatherServiceTests
    {
        private IWeatherService WeatherService { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            var config = new OpenWeatherSettings
            {
                Endpoint = "http://api.openweathermap.org/",
                ApiKey = "" // NOTICE: Put API key here to run tests.
            };

            var options = new OptionsWrapper<OpenWeatherSettings>(config);

            WeatherService = new WeatherService(httpClient, options);
        }

        [Test, Parallelizable]
        [TestCase("45416", "US", "US", "Trotwood", 39.8011, -84.2578, "45416")]
        [TestCase("TW8", "GB", "GB", "London", 51.4862, -0.3083, "TW8")]
        public async Task GetCoordinatesByPostalCode(string postCode, string countryCode, string country, string name, double lat, double lon, string expectedPostCode)
        {
            var result = await WeatherService.GetCoordinatesByPostalCode(postCode, countryCode);
            Assert.Multiple(() => {
                Assert.That(result is PostalCodeResult);
                Assert.That(result.Country, Is.EqualTo(country));
                Assert.That(result.Name, Is.EqualTo(name));
                Assert.That(result.Latitude, Is.EqualTo(lat));
                Assert.That(result.Longitude, Is.EqualTo(lon));
            });

            if (result is PostalCodeResult zipCodeResult)
            {
                Assert.That(zipCodeResult.PostCode, Is.EqualTo(expectedPostCode));
            }
        }

        [Test, Parallelizable]
        [TestCase("Trotwood", "OH", "US", "US", "Trotwood", 39.7972788, -84.3113334, "Ohio", 0, "", "")]
        [TestCase("London", "", "GB", "GB", "London", 51.5073219, -0.1276474, "England", 0, "London", "London")]
        public async Task GetCoordinateByCityState(string searchCity, string searchState, string searchCountry, string country, string name, double lat, double lon, string state, int nameCount, string feature_name, string ascii)
        {
            var result = await WeatherService.GetCoordinateByCityState(searchCity, searchState, searchCountry);
            Assert.Multiple(() => {
                Assert.That(result is LocationNameResult);
                Assert.That(result.Country, Is.EqualTo(country));
                Assert.That(result.Name, Is.EqualTo(name));
                Assert.That(result.Latitude, Is.EqualTo(lat));
                Assert.That(result.Longitude, Is.EqualTo(lon));
            });

            if (result is LocationNameResult locationNameResult)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(locationNameResult.State, Is.EqualTo(state));
                    Assert.That(locationNameResult.LocalNames, Has.Count.GreaterThanOrEqualTo(nameCount));
                    if (nameCount >= 2)
                    {
                        Assert.That(locationNameResult.LocalNames.ContainsKey(nameof(feature_name)), Is.True);
                        Assert.That(locationNameResult.LocalNames.ContainsKey(nameof(ascii)), Is.True);
                        Assert.That(locationNameResult.LocalNames[nameof(feature_name)], Is.EqualTo(feature_name));
                        Assert.That(locationNameResult.LocalNames[nameof(ascii)], Is.EqualTo(ascii));
                    }
                });
            }
        }
    }
}