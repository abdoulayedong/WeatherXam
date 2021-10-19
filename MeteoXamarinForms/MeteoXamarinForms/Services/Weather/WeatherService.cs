using System;
using System.Net.Http;
using System.Threading.Tasks;
using MeteoXamarinForms.Models;
using Newtonsoft.Json;

namespace MeteoXamarinForms.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly string apiKey = "";
        private readonly HttpClient _client;

        public WeatherService()
        {
            _client = new HttpClient();
        }

        public async Task<Root> GetWeatherFromLatLong(double latitude, double longitude)
        {
            var Url = String.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&exclude=minutely,alerts&units=metric&appid={2}", latitude, longitude, apiKey);
            
            var response = await _client.GetAsync(Url);

            var responseBody = await response.Content.ReadAsStringAsync();
             
            var responseObject = JsonConvert.DeserializeObject<Root>(responseBody);
            
            return responseObject;
        }
    }
}
