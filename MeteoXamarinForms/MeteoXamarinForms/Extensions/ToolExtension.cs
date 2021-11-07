using MeteoXamarinForms.Models;
using MeteoXamarinForms.Resx;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeteoXamarinForms.Extensions
{
    public static class ToolExtension
    {
        public async static Task<string> GetCountry(double latitude, double longitude)
        {
            places:
                var placemarks = await Task.FromResult(await Geocoding.GetPlacemarksAsync(latitude, longitude));
            var country = placemarks?.FirstOrDefault();
            if(country == null)
            {
                goto places;
            }
            else
            {
                return String.Format("{0}, {1}", country.AdminArea, country.CountryName);
            }
        }

        public static string GetTimezone(string timezone, string city)
        {
            var timezoneSplit = timezone.Split('/');
            return String.Format("{0}/{1}", timezoneSplit[0], StringExtensions.FirstCharToUpper(city));
        }

        public static DateTime GetDateTimeFromTimezone(int timezoneOffset)
        {
            return DateTime.UtcNow.AddSeconds(timezoneOffset);
        }

        public static DateTime GetDateTimeFromTimezone(DateTime dateTime, int timezoneOffset)
        {
            return dateTime.AddSeconds(timezoneOffset);
        }

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
            DateTime dateTime = new (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp, int timezoneOffset)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTime.AddSeconds(timezoneOffset);
        }

        public static string GetDayOfWeek(DateTime currentDate)
        {
            if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                return AppResources.Sunday;
            else if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                return AppResources.Saturday;
            else if (currentDate.DayOfWeek == DayOfWeek.Monday)
                return AppResources.Monday;
            else if (currentDate.DayOfWeek == DayOfWeek.Tuesday)
                return AppResources.Tuesday;
            else if (currentDate.DayOfWeek == DayOfWeek.Wednesday)
                return AppResources.Wednesday;
            else if (currentDate.DayOfWeek == DayOfWeek.Thursday)
                return AppResources.Thursday;
            else 
                return AppResources.Friday;
        }

        public static int MetreSecToKilometerHour(double wind_speed)
        {
            return (int)(wind_speed * 3.6);
        }

        public static int MilesHourToKilometerHour(double wind_speed)
        {
            return (int)(wind_speed * 1.60934);
        }

        public static string GetUviValue(double uvi)
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

        public static int RoundedTemperature(double temperature)
        {
            return (int)Math.Round(temperature, 0);
        }

        public static string GetIcon(string icon)
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
            string cityNameRemaster = cityName.Split('/')[1];
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), String.Format("{0}.txt", cityNameRemaster));
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
            var data = File.ReadAllText(fullFileName);
            return DeserializeWeatherData(data);
        }

        //public static void DeleteDataLocaly(string cityName)
        //{
        //    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), String.Format("{0}.txt", cityName));
        //    if(Preferences.Get("FullFileName", String.Empty).Contains(cityName))
        //    {
        //        Preferences.Remove("FullFileName");
        //    }
        //    File.Delete(fileName);
        //}

        public static void DeleteDataLocaly(Root city)
        {
            var firstString = SerializeWeatherData(city);
            var fileList = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            foreach (var file in fileList)
            {
                var secondString = SerializeWeatherData(GetDataLocaly(file));
                if (firstString.Equals(secondString))
                {
                    File.Delete(file);
                }
            }
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
