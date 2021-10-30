using System;
using System.Net.Http;
using System.Threading.Tasks;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.Resx;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MeteoXamarinForms.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly string apiKey = "bae28df78f81c27a2be7a7e3f1ef3c4e";
        private readonly HttpClient _client;

        public WeatherService()
        {
            _client = new HttpClient();
        }

        public async Task<Root> GetWeatherFromLatLong(double latitude, double longitude)
        {
            var pref = Preferences.Get("UnitParameter", "metric");

            var Url = String.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&exclude=minutely,alerts&appid={2}&lang={3}&units={4}", latitude, longitude, apiKey, AppResources.RequestLanguage, pref);
            
            var response = await _client.GetAsync(Url);

            var responseBody = await response.Content.ReadAsStringAsync();
             
            var responseObject = JsonConvert.DeserializeObject<Root>(responseBody);
            
            return responseObject;
        }
    }
}
