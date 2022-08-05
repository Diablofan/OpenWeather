using FluentValidation;
using OpenWeather.Pages;

namespace OpenWeather.Validation
{
    public class LookupValidator : AbstractValidator<Lookup>
    {
        public LookupValidator()
        {
            static bool using_coordinates(Lookup c) => string.IsNullOrEmpty(c.NameOrPostCode);
            const string ZipOrCityStateRegex = @"^([0-9]{5})(-[0-9]{4})?|((.*),\s+([A-Z]+))$";

            RuleFor(c => c.Longitude).InclusiveBetween(-180, +180).When(using_coordinates);
            RuleFor(c => c.Latitude).InclusiveBetween(-90, +90).When(using_coordinates);

            RuleFor(c => c.NameOrPostCode).Matches(ZipOrCityStateRegex).When(c => c.Longitude == 0 && c.Latitude == 0);
        }
    }
}
