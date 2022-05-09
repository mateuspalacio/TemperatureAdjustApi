using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TemperatureAdjustApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private HttpClient _httpClient = new HttpClient();
        private readonly string Ip = "http://192.168.0.175:3000";
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/roomtemperature")]
        public async Task<IActionResult> AddRoomTemperature([FromBody] WeatherForecastPost wf)
        {
            _httpClient.BaseAddress = new Uri(Ip);
            var toSend = JsonConvert.SerializeObject(wf);
            HttpContent content = new StringContent(toSend);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = await _httpClient.PostAsync("/temperatura", content);
            if(!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());
            return Ok(resp);
        }

        [HttpPost("/ventilador")]
        public async Task<IActionResult> Ventilador([FromBody] VentiladorPost wf)
        {
            _httpClient.BaseAddress = new Uri(Ip);
            var toSend = JsonConvert.SerializeObject(wf);
            HttpContent content = new StringContent(toSend, Encoding.UTF8, "application/json");
            var resp = await _httpClient.PostAsync("/ventilador", content);
            if (!resp.IsSuccessStatusCode)
                throw new Exception(resp.Content.ToString());
            return Ok(resp);
        }

        [HttpGet("/temperature")]
        public async Task<IActionResult> GetRoomTemperature()
        {
            _httpClient.BaseAddress = new Uri(Ip);
            var resp = await _httpClient.GetAsync("/temperatura");
            var response = JsonConvert.DeserializeObject<WeatherForecastGet>(await resp.Content.ReadAsStringAsync());
            if(response != null)
            {
                if (response.temperatura >= 30)
                {
                    VentiladorPost vp = new VentiladorPost
                    {
                        estado = true
                    };
                    var toSend = JsonConvert.SerializeObject(vp);
                    HttpContent content = new StringContent(toSend, Encoding.UTF8, "application/json");
                    var respVentiladorOn = await _httpClient.PostAsync("/ventilador", content);
                    if (!respVentiladorOn.IsSuccessStatusCode)
                        throw new Exception(await respVentiladorOn.Content.ReadAsStringAsync());
                    Console.WriteLine(respVentiladorOn.Content);
                    return Ok(respVentiladorOn.Content);
                }
                if (response.temperatura <= 25)
                {
                    VentiladorPost vp = new VentiladorPost
                    {
                        estado = false
                    };
                    var toSend = JsonConvert.SerializeObject(vp);
                    HttpContent content = new StringContent(toSend, Encoding.UTF8, "application/json");
                    var respVentiladorOn = await _httpClient.PostAsync("/ventilador", content);
                    if (!respVentiladorOn.IsSuccessStatusCode)
                        throw new Exception(await respVentiladorOn.Content.ReadAsStringAsync());
                    return Ok(respVentiladorOn);
                }
            }
            return Ok(resp);
        }
    }
}