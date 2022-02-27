namespace MorningApiApp.ExternalServices.OpenWeatherApi
{
    public static class OpenWeatherApiConstants
    {
        public static double latitudeDefault = latitudeBulgariaCenter;
        public static double longitudeDefault = longitudeBulgariaCenter;

        public static double latitudeBulgariaCenter = 42.766053;
        public static double longitudeBulgariaCenter = 25.238446;

        public static double latitudeBurgas = 42.493284;
        public static double longitudeBurgas = 27.472270;

        public static double latitudeBurgasELMS = 42.514083;
        public static double longitudeBurgasELMS = 27.469551;

        public static double latitudeCherniVrah = 42.563804;
        public static double longitudeCherniVrah = 23.278447;

        public static double latitudeSofia = 42.695823;
        public static double longitudeSofia = 23.332844;

        public static double latitude = latitudeBurgasELMS;
        public static double longitude = longitudeBurgasELMS;

        public static string Url = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}";
    }
}
