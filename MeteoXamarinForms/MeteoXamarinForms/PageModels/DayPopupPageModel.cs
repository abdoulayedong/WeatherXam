using MeteoXamarinForms.Models;
using MeteoXamarinForms.ViewModels.Base;
using System;
using MeteoXamarinForms.Extensions;
using Xamarin.CommunityToolkit.Helpers;

namespace MeteoXamarinForms.PageModels
{
    public class DayPopupPageModel : PageModelBase
    {
        public Daily DailyData { get; set; }
        public override void Init(object initData)
        {
            DailyData = initData as Daily;
            Initialize();
        }

        private void Initialize()
        {
            Day = ToolExtension.UnixTimeStampToDateTime(DailyData.Dt);
            DayOfWeek = new(() => ToolExtension.GetDayOfWeek(Day)); 
            Temperature = ToolExtension.RoundedTemperature(DailyData.Temp.Day);
            Icon = ToolExtension.GetIcon(DailyData.Weather[0].Icon);
            Probability = (int)(DailyData.Pop * 100);
            WindSpeed = ToolExtension.MetreSecToKilometerHour(DailyData.Wind_Speed);
            Humidity = DailyData.Humidity;
            UvIndex = ToolExtension.GetUviValue(DailyData.Uvi);
            Sunrise = ToolExtension.UnixTimeStampToDateTime(DailyData.Sunrise);
            Sunset = ToolExtension.UnixTimeStampToDateTime(DailyData.Sunset);
        }

        #region Properties
        private LocalizedString _dayOfWeek;
        public LocalizedString DayOfWeek
        {
            get { return _dayOfWeek; }
            set => SetProperty(ref _dayOfWeek, value);
        }

        private DateTime _day;
        public DateTime Day
        {
            get { return _day; }
            set => SetProperty(ref _day, value);
        }

        private int _temperature;
        public int Temperature
        {
            get { return _temperature; }
            set => SetProperty(ref _temperature, value);
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set => SetProperty(ref _icon, value);
        }

        private int _probability;
        public int Probability
        {
            get { return _probability; }
            set => SetProperty(ref _probability, value);
        }

        private int _windSpeed;
        public int WindSpeed
        {
            get { return _windSpeed; }
            set => SetProperty(ref _windSpeed, value);
        }

        private int _humidity;
        public int Humidity
        {
            get { return _humidity; }
            set => SetProperty(ref _humidity, value);
        }

        private string _uvIndex;
        public string UvIndex
        {
            get { return _uvIndex; }
            set => SetProperty(ref _uvIndex, value);
        }

        private DateTime _sunrise;
        public DateTime Sunrise
        {
            get { return _sunrise; }
            set => SetProperty(ref _sunrise, value);
        }

        private DateTime _sunset;
        public DateTime Sunset
        {
            get { return _sunset; }
            set => SetProperty(ref _sunset, value);
        }
        #endregion
    }
}
