using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly string Ip = "";
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
            var resp = await _httpClient.PostAsync(_httpClient.BaseAddress + "/temperatura", content);
            if(!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());
            return Ok(resp);
        }

        [HttpGet("/temperature")]
        public async Task<IActionResult> GetRoomTemperature()
        {
            var resp = await _httpClient.GetAsync(_httpClient.BaseAddress + "/temperatura");
            var response = JsonConvert.DeserializeObject<WeatherForecastGet>(await resp.Content.ReadAsStringAsync());
            if(response != null)
            {
                if (response.temperatura >= 30)
                {
                    VentiladorPost vp = new VentiladorPost
                    {
                        estado = true
                    };
                    _httpClient.BaseAddress = new Uri(Ip);
                    var toSend = JsonConvert.SerializeObject(vp);
                    HttpContent content = new StringContent(toSend);
                    var respVentiladorOn = await _httpClient.PostAsync(_httpClient.BaseAddress + "/temperatura", content);
                    if (!respVentiladorOn.IsSuccessStatusCode)
                        throw new Exception(await respVentiladorOn.Content.ReadAsStringAsync());
                    return Ok(respVentiladorOn);
                }
            }
            return Ok(resp);
        }
    }
}