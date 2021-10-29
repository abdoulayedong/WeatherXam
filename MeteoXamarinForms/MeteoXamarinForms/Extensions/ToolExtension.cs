using MeteoXamarinForms.Models;
using MeteoXamarinForms.Resx;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Linq;

namespace MeteoXamarinForms.Extensions
{
    public static class ToolExtension
    {
        public static bool ExistingWeatherData()
        {
            var directory = (Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))).ToList();

            if(directory.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
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
                return AppResources.Low;
            }
            else if (uvi >= 3 && uvi < 6)
            {
                return AppResources.Moderate;
            }
            else if (uvi >= 6 && uvi < 8)
            {
                return AppResources.High;
            }
            else if (uvi >= 8 && uvi < 11)
            {
                return AppResources.VeryHigh; 
            }
            else return AppResources.Extrem;
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

        public static Root GetLastRegisterWeatherData()
        {
            var files = Directory. GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            var fileName = String.Empty;
            foreach(var file in files)
            {
                if(fileName == string.Empty)
                {
                    fileName = file;
                }else if(File.GetLastWriteTime(file) < File.GetLastWriteTime(fileName))
                {
                    fileName = file;
                }
            }
            
            return GetDataLocaly(fileName);
        }
        public static Root GetDataLocaly(string fullFileName)
        {
            return DeserializeWeatherData(File.ReadAllText(fullFileName));
        }

        public static void DeleteDataLocaly(string cityName)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), String.Format("{0}.txt", cityName));
            if(Preferences.Get("FullFileName", String.Empty).Contains(cityName))
            {
                Preferences.Remove("FullFileName");
            }
            File.Delete(fileName);
        }

        public static string GetWindDirection(int wind_deg)
        {
            if(wind_deg >= 0 && wind_deg < 22.5)            
                return AppResources.North;
            else if(wind_deg >= 22.5 && wind_deg < 67.5)            
                return AppResources.NorthEast;
            else if (wind_deg >= 67.5 && wind_deg < 112.5)            
                return AppResources.East;
            else if (wind_deg >= 112.5 && wind_deg < 157.5)            
                return AppResources.SouthEast;
            else if (wind_deg >= 157.5 && wind_deg < 202.5)            
                return AppResources.South;
            else if (wind_deg >= 202.5 && wind_deg < 247.5)            
                return AppResources.SouthWest;
            else if (wind_deg >= 247.5 && wind_deg < 292.5)            
                return AppResources.West;
            else if (wind_deg >= 292.5 && wind_deg < 337.5)            
                return AppResources.NorthWest;
            else 
                return AppResources.North;
        }
    }
}
