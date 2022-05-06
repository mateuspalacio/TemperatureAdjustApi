namespace TemperatureAdjustApi
{
    public class WeatherForecastPost
    {
        public int temperatura { get; set; }
        public int? temperaturaAmbiente { get; set; }
        public int? temperatureMinima { get; set; }
    }

    public class WeatherForecastGet
    {
        public int temperatura { get; set; }
    }
    public class VentiladorPost
    {
        public bool estado { get; set; }
    }
    public class VentiladorGet
    {
        public bool estado { get; set; }
        public int temperatura { get; set; }
    }
}