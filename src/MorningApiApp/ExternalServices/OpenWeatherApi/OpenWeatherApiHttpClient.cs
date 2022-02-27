using MorningApiApp.ExternalServices.OpenWeatherApi.Enums;

namespace MorningApiApp.ExternalServices.OpenWeatherApi
{
    internal class OpenWeatherApiHttpClient : WpfHttpClient
    {
        internal string GetHttpResponse()
        {
            return base.GetHttpResponse(OpenWeatherApiConstants.Url);
        }

        internal string GetHttpResponse(WeatherCityEnum city)
        {
            return base.GetHttpResponse(GetUrlByCity(city));
        }

        private string GetUrlByCity(WeatherCityEnum city)
        {
            return city switch
            {
                WeatherCityEnum.Burgas => string.Format(OpenWeatherApiConstants.Url, OpenWeatherApiConstants.latitudeBurgas, OpenWeatherApiConstants.longitudeBurgas, Constants.Credentials.ExternalServices.OpenWeatherApi.ApiKey),
                WeatherCityEnum.Sofia => string.Format(OpenWeatherApiConstants.Url, OpenWeatherApiConstants.latitudeSofia, OpenWeatherApiConstants.longitudeSofia, Constants.Credentials.ExternalServices.OpenWeatherApi.ApiKey),
                WeatherCityEnum.CherniVrah => string.Format(OpenWeatherApiConstants.Url, OpenWeatherApiConstants.latitudeCherniVrah, OpenWeatherApiConstants.longitudeCherniVrah, Constants.Credentials.ExternalServices.OpenWeatherApi.ApiKey),
                WeatherCityEnum.BulCenter => string.Format(OpenWeatherApiConstants.Url, OpenWeatherApiConstants.latitudeBulgariaCenter, OpenWeatherApiConstants.longitudeBulgariaCenter, Constants.Credentials.ExternalServices.OpenWeatherApi.ApiKey),
                _ => string.Format(OpenWeatherApiConstants.Url, OpenWeatherApiConstants.latitudeDefault, OpenWeatherApiConstants.latitudeDefault, Constants.Credentials.ExternalServices.OpenWeatherApi.ApiKey)
            };
        }
    }
}
