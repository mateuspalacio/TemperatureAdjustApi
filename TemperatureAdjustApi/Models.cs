namespace TemperatureAdjustApi
{
    public class WeatherForecastPost
    {
        public double temperatura { get; set; }
        public double? temperaturaAmbiente { get; set; }
        public double? temperatureMinima { get; set; }
    }

    public class WeatherForecastGet
    {
        public double temperatura { get; set; }
    }
    public class VentiladorPost
    {
        public bool estado { get; set; }
    }
    public class VentiladorGet
    {
        public bool estado { get; set; }
        public double temperatura { get; set; }
    }
}