using MeteoXamarinForms.Models;
using MeteoXamarinForms.ViewModels.Base;
using System;
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
using MeteoXamarinForms.Services.Toast;
using MeteoXamarinForms.Resx;
using FreshMvvm;
using MeteoXamarinForms.Services;
using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace MeteoXamarinForms.ViewModels
{
    public class WeatherPageModel : PageModelBase
    {
        #region Constructor
        public WeatherPageModel()
        {
            _weatherService = FreshIOC.Container.Resolve<IWeatherService>();

            DailyDetailCommand = new Command<DayPrevision>(
            async (DayPrevision dayPrevision) =>
            {
                Daily selectedDay = Weather.Daily.Where(day => ToolExtension.GetDayOfWeek(ToolExtension.UnixTimeStampToDateTime(day.Dt)) == dayPrevision.DaysOfWeek.Localized).FirstOrDefault();
                await CoreMethods.PushPopupPageModel<DayPopupPageModel>(data: selectedDay);
            });

            AddWeatherInformationCommand = new Command(
                async () =>
                {
                    await CoreMethods.DisplayActionSheet("", "Cancel", "destroy");
                });

            OpenCityManagementCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<CityPageModel>(data: true, animate: false);
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

            IsSpanish = Preferences.Get("Language", "en") == "es" ? true : false;

            Application.Current.RequestedThemeChanged += (sender, args) =>
            {
                dynamic result = ThemeEvaluation(args);
                LineChart.BackgroundColor = SKColor.Parse(result.hexBackgroundColor);
                LineChart.Entries.ForEach(entry => entry.ValueLabelColor = SKColor.Parse(result.hexValueLabelColor));
            };
        }
        #endregion

        #region Properties
        private int _themeNumber = Application.Current.RequestedTheme == OSAppTheme.Light ? 1 : 2;

        public int ThemeNumber
        {
            get => _themeNumber; 
            set => SetProperty (ref _themeNumber, value); 
        }

        private LineChart lineChart;
        public LineChart LineChart 
        { 
            get => lineChart; 
            set => SetProperty(ref lineChart, value); 
        }

        private string[] Days = new string[7];
        private int[] Temperatures = new int[7];
        private SKColor orangeColor = SKColor.Parse("#C06048");

        public Root Weather { get; set; }
        private readonly IWeatherService _weatherService;

        public bool IsSpanish { get; set; } = false;

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set => SetProperty(ref _isRefreshing, value);
        }

        private bool _isLocalPosition;
        public bool IsLocalPosition
        {
            get { return _isLocalPosition; }
            set => SetProperty(ref _isLocalPosition, value);
        }

        private int _rotationDegree;
        public int RotationDegree
        {
            get { return _rotationDegree; }
            set => SetProperty(ref _rotationDegree, value);
        }

        private DateTime _updateDate;
        public DateTime UpdateDate
        {
            get { return _updateDate; }
            set => SetProperty(ref _updateDate, value);
        }

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set => SetProperty(ref _cityName, value);
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
        private dynamic ThemeEvaluation(dynamic theme)
        {
            dynamic backgroundColor;
            dynamic valueLabelColor;
            if (theme.RequestedTheme == OSAppTheme.Light)
            {
                backgroundColor = Application.Current.Resources["MainFrameColor"];
                valueLabelColor = Application.Current.Resources["MainFrameColorDark"];
            }
            else
            {
                backgroundColor = Application.Current.Resources["MainFrameColorDark"];
                valueLabelColor = Application.Current.Resources["MainFrameColor"];
            }

            ThemeNumber = theme.RequestedTheme == OSAppTheme.Light ? 1 : 2;

            Color myBackgroundColor = Color.FromRgba(backgroundColor.R, backgroundColor.G, backgroundColor.B, backgroundColor.A);
            Color myValueLabelColor = Color.FromRgba(valueLabelColor.R, valueLabelColor.G, valueLabelColor.B, valueLabelColor.A);
            string hexBackgroundColor = myBackgroundColor.ToHex();
            string hexValueLabelColor = myValueLabelColor.ToHex();
            return new { hexBackgroundColor,  hexValueLabelColor };
        } 

        private async Task Update()
        {
            try
            {
                Weather = await _weatherService.GetWeatherFromLatLong(Weather.Lat, Weather.Lon);
                Weather.Timezone = Preferences.Get("CurrentTimezone", "");
                await SQLiteDataContext.Instance.UpdateRootAsync(Weather);
                SetUiData();
                DependencyService.Get<IToastService>().ShortToast(AppResources.UpdatedData);
            }
            catch (Exception)
            {
                DependencyService.Get<IToastService>().ShortToast(AppResources.NoInternet);
            }
        }
        private void SetUiData()
        {
            Preferences.Set("Lat", Weather.Lat);
            Preferences.Set("Lon", Weather.Lon);

            // Current day weather
            IsLocalPosition = Preferences.Get("LocalTimezone", "") == Weather.Timezone ?  true :  false;
            var current = Weather.Current;
            var currentDay = Weather.Daily[0];
            var hourlyForecast = Weather.Hourly;
            var dailyForecast = Weather.Daily;
            Preferences.Set("TimezoneOffset", Weather.Timezone_Offset);
            CurrentTemperature = ToolExtension.RoundedTemperature(current.Temp);
            Description = current.Weather[0].Description;
            MaxTemperature = ToolExtension.RoundedTemperature(currentDay.Temp.Max);
            MinTemperature = ToolExtension.RoundedTemperature(currentDay.Temp.Min);
            CurrentIcon = ToolExtension.GetIcon(current.Weather[0].Icon);
            FeelsLike = ToolExtension.RoundedTemperature(current.Feels_Like);

            // Hourly weather
            HourPrevisions = new ObservableCollection<HourPrevision>();
            for (int i = 0; i < 24; i++)
            {
                HourPrevision hourPrevision = new();
                DateTime dateTime = ToolExtension.UnixTimeStampToDateTime(hourlyForecast[i].Dt);
                hourPrevision.Hour = ToolExtension.GetDateTimeFromTimezone(dateTime, Weather.Timezone_Offset);
                hourPrevision.Icon = ToolExtension.GetIcon(hourlyForecast[i].Weather[0].Icon);
                hourPrevision.Temperature = ToolExtension.RoundedTemperature(hourlyForecast[i].Temp);
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
            DayPrevisions = new ();
            dynamic result = ThemeEvaluation(Application.Current);
            var turnoverEntries = new List<ChartEntry>();
            for (int i = 0; i < 7; i++)
            {
                DayPrevision dayPrevision = new ();
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
                dayPrevision.MaxTemperature = ToolExtension.RoundedTemperature(dailyForecast[i].Temp.Max);
                dayPrevision.MinTemperature = ToolExtension.RoundedTemperature(dailyForecast[i].Temp.Min);
                dayPrevision.DayIcon = ToolExtension.GetIcon(dailyForecast[i].Weather[0].Icon);
                DateTime dateTime = ToolExtension.UnixTimeStampToDateTime(dailyForecast[i].Dt);
                DateTime dateTimeUtc = ToolExtension.GetDateTimeFromTimezone(dateTime, Weather.Timezone_Offset);
                dayPrevision.DaysOfWeek = new(() => ToolExtension.GetDayOfWeek(dateTimeUtc));
                var data = (float)dailyForecast[i].Rain;
                turnoverEntries.Add(new ChartEntry(data)
                {
                    Color = orangeColor,
                    Label = dayPrevision.DaysOfWeek.Localized.Substring(0,3),
                    ValueLabel = $"{data } mm",
                    ValueLabelColor = SKColor.Parse(result.hexValueLabelColor)
                });

                DayPrevisions.Add(dayPrevision);
            }

            LineChart = new LineChart {
                Entries = turnoverEntries,
                IsAnimated = true,
                LabelTextSize = 30f,
                BackgroundColor = SKColor.Parse(result.hexBackgroundColor),
                LabelOrientation = Orientation.Horizontal, 
                ValueLabelOrientation = Orientation.Vertical,
                LabelColor = SKColor.Parse("#999999"),
                PointSize = 20,
            };

            // More daily information
            UvIndex = new(() => ToolExtension.GetUviValue(current.Uvi));
            Sunrise = ToolExtension.UnixTimeStampToDateTime(current.Sunrise, Weather.Timezone_Offset);
            Sunset = ToolExtension.UnixTimeStampToDateTime(current.Sunset, Weather.Timezone_Offset);
            var pref = Preferences.Get("UnitParameter", "metric");
            if (pref == "imperial")
            {
                WindSpeed = ToolExtension.MilesHourToKilometerHour(current.Wind_Speed);
            }else
            {
                WindSpeed = ToolExtension.MetreSecToKilometerHour(current.Wind_Speed);
            }
            WindDeg = current.Wind_Deg;
            WindDirection = new(() => ToolExtension.GetWindDirection(WindDeg));
            Clouds = current.Clouds;
            Humidity = current.Humidity;
            UpdateDate = ToolExtension.UnixTimeStampToDateTime(Weather.Current.Dt);
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

        #region Override Methods
        public async override void Init(object initData)
        {
            Weather = initData as Root;
            string[] city = Weather.Timezone.Split('/');
            CityName = city[city.Length - 1];
            TimeSpan time = DateTime.Now - ToolExtension.UnixTimeStampToDateTime(Weather.Current.Dt);
            //#if DEBUG
            //    time = TimeSpan.FromHours(0.6);
            //#else
            //    time = TimeSpan.FromSeconds(1000);    
            //#endif
            if (time.TotalMinutes > 30)
            {
                SetUiData();
                IsRefreshing = true;
                await Update();
                IsRefreshing = false;
            }               
            else
                SetUiData();
        }
        public async override void ReverseInit(object returnedData)
        {
            Weather = returnedData as Root;
            string[] city = Weather.Timezone.Split('/');
            CityName = city[city.Length - 1];
            TimeSpan time = DateTime.Now - ToolExtension.UnixTimeStampToDateTime(Weather.Current.Dt);
            //#if DEBUG
            //    time = TimeSpan.FromHours(0.6);
            //#else
            //    time = TimeSpan.FromSeconds(1000);    
            //#endif
            if (time.TotalMinutes > 30)
            {
                SetUiData();
                IsRefreshing = true;
                await Update();
                IsRefreshing = false;
            }
            else
                SetUiData();
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
