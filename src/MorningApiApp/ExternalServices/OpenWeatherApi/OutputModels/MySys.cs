using System;

namespace MorningApiApp.ExternalServices.OpenWeatherApi.OutputModels
{
    public class MySys
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string Country { get; set; }

        public DateTime Sunrise { get; set; }

        public DateTime Sunset { get; set; }
    }
}
