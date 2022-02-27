using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace MorningApiApp.ExternalServices.OpenWeatherApi.OutputModels
{
    public class MyRootWeatherModel
    {
        public MyCoordinates Coordinates { get; set; }

        public List<MyWeather> WeatherList { get; set; }

        public string Base { get; set; }

        public MyMain Main { get; set; }

        public int Visibility { get; set; }

        public MyWind Wind { get; set; }

        public MyClouds Clouds { get; set; }

        public DateTime TimeNow { get; set; }

        public MySys Sys { get; set; }

        public int TimeZone { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Cod { get; set; }

        public (Color ForegroundColor, Color BackgroundColor) GetColorByTemperature()
        {
            if (this.Main.Temp < -10)
            {
                return (Colors.White, Colors.Blue);
            }
            if (this.Main.Temp >= -10 && this.Main.Temp < 0)
            {
                return (Colors.Black, Colors.Cyan);
            }
            else if (this.Main.Temp >= 0 && this.Main.Temp < 10)
            {
                return (Colors.White, Colors.Green);
            }
            else if (this.Main.Temp >= 10 && this.Main.Temp < 20)
            {
                return (Colors.Black, Colors.YellowGreen);
            }
            else if (this.Main.Temp >= 20 && this.Main.Temp < 30)
            {
                return (Colors.Black, Colors.Yellow);
            }
            else if (this.Main.Temp >= 30)
            {
                return (Colors.Black, Colors.Magenta);
            }
            else
            {
                return (Colors.Black, Colors.Gray);
            }
        }

        public override string ToString()
        {
            return
                $"{Sys.Country} / {Name} ({Coordinates.Latitude:F3}, {Coordinates.Longitude:F3})\n" +
                $"{TimeNow.Date:yyyy-MM-dd} | {TimeNow.Hour:D2}:{TimeNow.Minute:D2}:{TimeNow.Second:D2}\n" +
                $"{new string('═', 32)}\n" +
                $"{WeatherList.FirstOrDefault().Main} ({WeatherList.FirstOrDefault().Description})\n" +
                $"🌡=: {Main.Temp:F1}°C (🌡~: {Main.TempFeelsLike:F1}°C)\n" +
                //$"Temp Min: {this.Main.TempMin:F1}°C\n" +
                //$"Temp Max: {this.Main.TempMax:F1}°C\n" +
                $"☟: {Main.Pressure} hPa\n" +
                $"💧: {Main.Humidity}%\n" +
                $"💨: {Wind.Speed} mps, {Wind.DirectionString}\n" +
                $"☁️: {Clouds.All}%\n" +
                $"🌄▲: {Sys.Sunrise.Hour:D2}:{Sys.Sunrise.Minute:D2} | 🌄▼: {Sys.Sunset.Hour:D2}:{Sys.Sunset.Minute:D2}\n";
        }
    }
}
