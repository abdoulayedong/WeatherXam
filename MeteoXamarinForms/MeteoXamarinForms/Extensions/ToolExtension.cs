using MeteoXamarinForms.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Essentials;

namespace MeteoXamarinForms.Extensions
{
    public static class ToolExtension
    {
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static int MetreSecToKilometerHour(double wind_speed)
        {
            return (int)(wind_speed * 3.6);
        }

        public static string getUviValue(double uvi)
        {
            if (uvi >= 0 && uvi < 3)
            {
                return "Low";
            }
            else if (uvi >= 3 && uvi < 6)
            {
                return "Moderate";
            }
            else if (uvi >= 6 && uvi < 8)
            {
                return "High";
            }
            else if (uvi >= 8 && uvi < 11)
            {
                return "Very High";
            }
            else return "Extrem";
        }

        public static int roundedTemperature(double temperature)
        {
            return (int)Math.Round(temperature, 0);
        }

        public static string getIcon(string icon)
        {
            return String.Format("_{0}.png", icon);
        }

        public static string SerializeWeatherData(Root weatherData)
        {
            return JsonConvert.SerializeObject(weatherData);
        }

        public static Root DeserializeWeatherData(string serialiseData)
        {
            return JsonConvert.DeserializeObject<Root>(serialiseData);
        }

        public static string GetCityName(string Timezone)
        {
            var separateName = Timezone.Split('/');
            return StringExtensions.FirstCharToUpper(separateName[1]);
        }

        public static void SaveDataLocaly(Root weatherData, string cityName)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), String.Format("{0}.txt", cityName));
            File.WriteAllText(fileName, ToolExtension.SerializeWeatherData(weatherData));
        }

        public static Root GetDataLocaly(string cityName)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), String.Format("{0}.txt", cityName));
            return DeserializeWeatherData(File.ReadAllText(fileName)); 
        }
    }
}
