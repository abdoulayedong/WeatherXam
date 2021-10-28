using MeteoXamarinForms.Models;
using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using FreshMvvm.Popups;
using MeteoXamarinForms.PageModels;
using System.Linq;
using MeteoXamarinForms.Extensions;
using Xamarin.CommunityToolkit.Helpers;
using System.Threading.Tasks;
using MeteoXamarinForms.Services.Weather;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace MeteoXamarinForms.ViewModels
{
    public class WeatherPageModel : PageModelBase
    {
        public WeatherPageModel()
        {
            _weatherService = ViewModelLocator.Resolve<IWeatherService>();

            DailyDetailCommand = new Command<DayPrevision>(
            async (DayPrevision dayPrevision) =>
            {
                var selectedDay = Weather.Daily.Where(day => ToolExtension.UnixTimeStampToDateTime(day.Dt) == dayPrevision.DaysOfWeek).FirstOrDefault();
                await CoreMethods.PushPopupPageModel<DayPopupPageModel>(data:selectedDay);
            });

            AddWeatherInformationCommand = new Command(
                async () =>
                {
                    await CoreMethods.DisplayActionSheet("", "Cancel", "destroy");
                });

            OpenCityManagementCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<CityPageModel>(data:true, animate:false);
                });

            OpenParameterCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<SettingPageModel>(animate: false);
                });

            ActualizeDataCommand = new Command(
                async () =>
                {
                    IsRefreshing = true;
                    await Update();
                    IsRefreshing = false;
                });
        }

        #region Properties
        public Root Weather { get; set; }
        private readonly IWeatherService _weatherService;

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set => SetProperty(ref _isRefreshing, value);
        }

        private int _rotationDegree;
        public int RotationDegree
        {
            get { return _rotationDegree; }
            set => SetProperty(ref _rotationDegree, value);
        }

        private DateTime _updateData;
        public DateTime UpdateData
        {
            get { return _updateData; }
            set => SetProperty(ref _updateData, value);
        }

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set => SetProperty(ref _description, value);
        }

        private int _currentTemperature;
        public int CurrentTemperature
        {
            get { return _currentTemperature; }
            set => SetProperty(ref _currentTemperature, value);
        }

        private string _currentIcon;
        public string CurrentIcon
        {
            get { return _currentIcon; }
            set => SetProperty(ref _currentIcon, value);
        }

        private int _minTemperature;
        public int MinTemperature
        {
            get { return _minTemperature; }
            set => SetProperty(ref _minTemperature, value);
        }

        private int _maxTemperature;
        public int MaxTemperature
        {
            get { return _maxTemperature; }
            set => SetProperty(ref _maxTemperature, value);
        }

        private int _feelsLike;
        public int FeelsLike
        {
            get { return _feelsLike; }
            set => SetProperty(ref _feelsLike, value);
        }

        private LocalizedString _uvIndex;
        public LocalizedString UvIndex
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

        private int _humidity;
        public int Humidity
        {
            get { return _humidity; }
            set => SetProperty(ref _humidity, value);
        }

        private int _windSpeed;
        public int WindSpeed
        {
            get { return _windSpeed; }
            set => SetProperty(ref _windSpeed, value);
        }

        private int _windDeg;
        public int WindDeg
        {
            get { return _windDeg; }
            set => SetProperty(ref _windDeg, value);
        }

        private LocalizedString _windDirection;
        public LocalizedString WindDirection
        {
            get { return _windDirection; }
            set => SetProperty(ref _windDirection, value);
        }

        private int _clouds;
        public int Clouds
        {
            get { return _clouds; }
            set => SetProperty(ref _clouds, value);
        }

        private ObservableCollection<HourPrevision> _hourPrevisions;

        public ObservableCollection<HourPrevision> HourPrevisions
        {
            get => _hourPrevisions;
            set => SetProperty(ref _hourPrevisions, value);
        }

        private ObservableCollection<DayPrevision> _dayPrevisions;

        public ObservableCollection<DayPrevision> DayPrevisions
        {
            get => _dayPrevisions;
            set => SetProperty(ref _dayPrevisions, value);
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            var city = Weather.Timezone.Split('/');
            CityName = city[city.Length - 1];
            SetUiData();
        }

        private async Task Update()
        {
            Weather = await _weatherService.GetWeatherFromLatLong(Weather.Lat, Weather.Lon);
            Weather.Timezone = CityName;
            ToolExtension.SaveDataLocaly(Weather, CityName);
            SetUiData();
        }

        private void SetUiData()
        {
            // Current day weather
            var current = Weather.Current;
            var currentDay = Weather.Daily[0];
            var hourlyForecast = Weather.Hourly;
            var dailyForecast = Weather.Daily;

            CurrentTemperature = ToolExtension.roundedTemperature(current.Temp);
            Description = current.Weather[0].Description;
            MaxTemperature = ToolExtension.roundedTemperature(currentDay.Temp.Max);
            MinTemperature = ToolExtension.roundedTemperature(currentDay.Temp.Min);
            CurrentIcon = ToolExtension.getIcon(current.Weather[0].Icon);
            FeelsLike = ToolExtension.roundedTemperature(current.Feels_Like);

            // Hourly weather
            HourPrevisions = new ObservableCollection<HourPrevision>();
            for (int i = 0; i < 24; i++)
            {
                HourPrevision hourPrevision = new();
                hourPrevision.Hour = ToolExtension.UnixTimeStampToDateTime(hourlyForecast[i].Dt);
                hourPrevision.Icon = ToolExtension.getIcon(hourlyForecast[i].Weather[0].Icon);
                hourPrevision.Temperature = ToolExtension.roundedTemperature(hourlyForecast[i].Temp);
                hourPrevision.ProbalilityOfPrecipitation = (int)(hourlyForecast[i].Pop * 100);
                if (hourPrevision.ProbalilityOfPrecipitation >= 0 && hourPrevision.ProbalilityOfPrecipitation <= 20)
                {
                    hourPrevision.ProbabilityIcon = "waterdrop1.png";
                }
                else if (hourPrevision.ProbalilityOfPrecipitation > 20 && hourPrevision.ProbalilityOfPrecipitation <= 60)
                {
                    hourPrevision.ProbabilityIcon = "waterdrop2.png";
                }
                else if (hourPrevision.ProbalilityOfPrecipitation > 60 && hourPrevision.ProbalilityOfPrecipitation <= 100)
                {
                    hourPrevision.ProbabilityIcon = "waterdrop3.png";
                }
                HourPrevisions.Add(hourPrevision);
            }

            // Daily weather
            DayPrevisions = new ObservableCollection<DayPrevision>();
            for (int i = 0; i < 7; i++)
            {
                DayPrevision dayPrevision = new DayPrevision();
                dayPrevision.ProbalilityOfPrecipitation = (int)(dailyForecast[i].Pop * 100);
                if (dayPrevision.ProbalilityOfPrecipitation >= 0 && dayPrevision.ProbalilityOfPrecipitation <= 20)
                {
                    dayPrevision.ProbabilityIcon = "waterdrop1.png";
                }
                else if (dayPrevision.ProbalilityOfPrecipitation > 20 && dayPrevision.ProbalilityOfPrecipitation <= 60)
                {
                    dayPrevision.ProbabilityIcon = "waterdrop2.png";
                }
                else if (dayPrevision.ProbalilityOfPrecipitation > 60 && dayPrevision.ProbalilityOfPrecipitation <= 100)
                {
                    dayPrevision.ProbabilityIcon = "waterdrop3.png";
                }
                dayPrevision.MaxTemperature = ToolExtension.roundedTemperature(dailyForecast[i].Temp.Max);
                dayPrevision.MinTemperature = ToolExtension.roundedTemperature(dailyForecast[i].Temp.Min);
                dayPrevision.DayIcon = ToolExtension.getIcon(dailyForecast[i].Weather[0].Icon);
                dayPrevision.DaysOfWeek = ToolExtension.UnixTimeStampToDateTime(dailyForecast[i].Dt);
                DayPrevisions.Add(dayPrevision);
            }

            // More daily information
            //UvIndex = ToolExtension.getUviValue(current.Uvi);
            UvIndex = new(() => ToolExtension.getUviValue(current.Uvi));
            Sunrise = ToolExtension.UnixTimeStampToDateTime(current.Sunrise);
            Sunset = ToolExtension.UnixTimeStampToDateTime(current.Sunset);
            WindSpeed = ToolExtension.MetreSecToKilometerHour(current.Wind_Speed);
            WindDeg = current.Wind_Deg;
            WindDirection = new(() => ToolExtension.GetWindDirection(WindDeg));
            Clouds = current.Clouds;
            Humidity = current.Humidity;
            UpdateData = ToolExtension.UnixTimeStampToDateTime(Weather.Current.Dt);
        }

        public override void Init(object initData)
        {
            Weather = initData as Root;
            Initialize();
        }

        private async void BackPressMethod()
        {
            var fullFileName = Preferences.Get("FullFileName", String.Empty);
            if (fullFileName != String.Empty)
            {
                var weatherData = ToolExtension.GetDataLocaly(fullFileName);
                await CoreMethods.PushPageModel<WeatherPageModel>(animate: false, data: weatherData);
            }
        }
        #endregion

        #region Commands
        public ICommand DailyDetailCommand { private set; get; }
        public ICommand AddWeatherInformationCommand { private set; get; }
        public ICommand OpenCityManagementCommand { private set; get; }
        public ICommand OpenParameterCommand { private set; get; }
        public ICommand ActualizeDataCommand { private set; get; }
        public ICommand BackPressCommand => new Command(BackPressMethod);
        #endregion
    }
}
