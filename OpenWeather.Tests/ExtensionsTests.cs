namespace OpenWeather.Tests
{
    public class ExtensionsTests
    { 
        [Test, Parallelizable]
        [TestCase(0, 1970, 1, 1, 0, 0, 0)]
        [TestCase(100, 1970, 1, 1, 0, 1, 40)]
        [TestCase(957443115, 2000, 5, 4, 12, 25, 15)]
        [TestCase(1659567956, 2022, 8, 3, 23, 05, 56)]
        [TestCase(1842992919, 2028, 05, 26, 22, 28, 39)]
        [TestCase(1156539365, 2006, 08, 25, 20, 56, 05)]
        [TestCase(1567698941, 2019, 09, 05, 15, 55, 41)]
        [TestCase(1698371199, 2023, 10, 27, 01, 46, 39)]
        [TestCase(1264695291, 2010, 01, 28, 16, 14, 51)]
        [TestCase(1116660993, 2005, 05, 21, 07, 36, 33)]
        [TestCase(1600844971, 2020, 09, 23, 07, 09, 31)]
        [TestCase(1243330441, 2009, 05, 26, 09, 34, 01)]
        [TestCase(1643334466, 2022, 01, 28, 01, 47, 46)]
        [TestCase(1478292427, 2016, 11, 04, 20, 47, 07)]
        public void FromUnixTimeStamp(long unixTimeStamp, int year, int month, int day, int hour, int minute, int second)
        {
            // According to unixtimestamp.com, the number below should be 2000-05-04 at 12:25:15 PM UTC
            var result = Extensions.FromUnixTimeStamp(unixTimeStamp);

            Assert.That(result, Is.EqualTo(new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc)));
        }

        [Test, Parallelizable]
        [TestCase(272.15, 32)]
        [TestCase(0, -459.67)]
        [TestCase(373.15, 212)]
        [TestCase(297.038889, 75)]
        public void ToFahrenheit(double kelvin, double fahrenheit)
        {
            var result = Extensions.ToFahrenheit(kelvin);
            Assert.That(result - fahrenheit, Is.LessThan(0.001));
        }
    }
}
