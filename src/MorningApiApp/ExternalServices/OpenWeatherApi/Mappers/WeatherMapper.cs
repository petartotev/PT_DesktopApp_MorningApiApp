using System;
using System.Linq;
using MorningApiApp.ExternalServices.OpenWeatherApi.Models;
using MorningApiApp.ExternalServices.OpenWeatherApi.OutputModels;

namespace MorningApiApp.ExternalServices.OpenWeatherApi.Mappers
{
    public static class WeatherMapper
    {
        public static MyRootWeatherModel ToMyRootWeatherModel(this RootWeather model)
        {
            return new MyRootWeatherModel
            {
                Coordinates = new MyCoordinates
                {
                    Latitude = model.coord.lat,
                    Longitude = model.coord.lon,
                },
                WeatherList = model.weather.Select(
                    x => new MyWeather
                    {
                        Description = x.description,
                        Icon = x.icon,
                        Id = x.id,
                        Main = x.main
                    }).ToList(),
                Base = model.@base,
                Main = new MyMain
                {
                    Temp = GetCelsiusFromKelvin(model.main.temp),
                    TempMin = GetCelsiusFromKelvin(model.main.temp_min),
                    TempMax = GetCelsiusFromKelvin(model.main.temp_max),
                    TempFeelsLike = GetCelsiusFromKelvin(model.main.feels_like),
                    Pressure = model.main.pressure,
                    Humidity = model.main.humidity
                },
                Visibility = model.visibility,
                Wind = new MyWind
                {
                    Speed = model.wind.speed,
                    DirectionDegrees = model.wind.deg,
                    DirectionString = GetWeatherDirectionStringByDegrees(model.wind.deg)
                },
                Clouds = new MyClouds
                {
                    All = model.clouds.all
                },
                TimeNow = GetDateTimeByUnixTimeStamp(model.dt),
                Sys = new MySys
                {
                    Country = model.sys.country,
                    Sunrise = GetDateTimeByUnixTimeStamp(model.sys.sunrise),
                    Sunset = GetDateTimeByUnixTimeStamp(model.sys.sunset),
                    Id = model.sys.id,
                    Type = model.sys.type
                },
                TimeZone = model.timezone,
                Id = model.id,
                Name = model.name,
                Cod = model.cod
            };
        }

        private static DateTime GetDateTimeByUnixTimeStamp(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private static string GetWeatherDirectionStringByDegrees(int degree)
        {
            if (degree >= 348.75 || degree < 11.25)
            {
                return "N";
            }
            else if (degree >= 11.25 && degree < 33.75)
            {
                return "NNE";
            }
            else if (degree >= 33.75 && degree < 56.25)
            {
                return "NE";
            }
            else if (degree >= 56.25 && degree < 78.75)
            {
                return "ENE";
            }
            else if (degree >= 78.75 && degree < 101.25)
            {
                return "E";
            }
            else if (degree >= 101.25 && degree < 123.75)
            {
                return "ESE";
            }
            else if (degree >= 123.75 && degree < 146.25)
            {
                return "SE";
            }
            else if (degree >= 146.25 && degree < 168.75)
            {
                return "SSE";
            }
            else if (degree >= 168.75 && degree < 191.25)
            {
                return "S";
            }
            else if (degree >= 191.25 && degree < 213.75)
            {
                return "SSW";
            }
            else if (degree >= 213.75 && degree < 236.25)
            {
                return "SW";
            }
            else if (degree >= 236.25 && degree < 258.75)
            {
                return "WSW";
            }
            else if (degree >= 258.75 && degree < 281.25)
            {
                return "W";
            }
            else if (degree >= 281.25 && degree < 303.75)
            {
                return "WNW";
            }
            else if (degree >= 303.75 && degree < 326.25)
            {
                return "NW";
            }
            else if (degree >= 326.25 && degree < 348.75)
            {
                return "NNW";
            }
            else
            {
                return "Default";
            }
        }

        private static double GetCelsiusFromFahrenheit(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        private static double GetCelsiusFromKelvin(double kelvin)
        {
            return kelvin - 273.15;
        }
    }
}
