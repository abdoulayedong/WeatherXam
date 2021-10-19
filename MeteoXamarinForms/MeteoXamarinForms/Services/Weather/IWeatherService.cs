using MeteoXamarinForms.Models;
using System.Threading.Tasks;

namespace MeteoXamarinForms.Services.Weather
{
    public interface IWeatherService
    {
        Task<Root> GetWeatherFromLatLong(double latitude, double longitude);
    }
}