using MeteoXamarinForms.Services.Weather;
using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using MeteoXamarinForms.Extensions;
using FreshMvvm;

namespace MeteoXamarinForms.ViewModels
{
    public class SearchPageModel : PageModelBase
    {
        private readonly IWeatherService _weatherService;
        public bool IsModalView { get; set; } = false;  
        public SearchPageModel()
        {
            _weatherService = FreshIOC.Container.Resolve<IWeatherService>();      
            GoToWeatherCommand = new Command(
                async () =>
                {
                    IsActivatedSearch = true;
                    IsNotFoundLocation = false;
                    SearchInProgress = true;
                    var currentLocation = await Geolocation.GetLocationAsync();
                    if (currentLocation is null)
                    {
                        IsNotFoundLocation = true;
                        IsActivatedSearch = false;
                        SearchInProgress = false;
                    }
                    else
                    {
                        IsNotFoundLocation = false;
                        var weatherData = await _weatherService.GetWeatherFromLatLong(currentLocation.Latitude, currentLocation.Longitude);
                        ToolExtension.SaveDataLocaly(weatherData, ToolExtension.GetCityName(weatherData.Timezone));
                        Preferences.Set("CurrentLocation", ToolExtension.GetCityName(weatherData.Timezone));
                        await CoreMethods.PushPageModel<WeatherPageModel>(data: weatherData);
                        SearchInProgress = false;
                        IsActivatedSearch = false;
                    }
                });

            PerformSearch = new Command<string>(
                async (string query) =>
                {
                    IsActivatedSearch = true;
                    IsNotFoundLocation = false;
                    SearchInProgress = true;
                    var location = await GetLocation(query);
                    if(location is null)
                    {
                        IsNotFoundLocation = true;
                        SearchInProgress = false;
                        IsActivatedSearch = false;
                    }
                    else
                    {
                        IsNotFoundLocation = false;
                        var weatherData = await _weatherService.GetWeatherFromLatLong(location.Latitude, location.Longitude);
                        var timezoneRemake = weatherData.Timezone.Split('/');
                        weatherData.Timezone = String.Format("{0}/{1}", timezoneRemake[0], StringExtensions.FirstCharToUpper(query));
                        ToolExtension.SaveDataLocaly(weatherData, weatherData.Timezone);
                        if (IsModalView)
                        {
                            await CoreMethods.PushPageModel<WeatherPageModel>(data: weatherData);
                        }
                        else
                        {
                            await CoreMethods.PushPageModel<WeatherPageModel>(data:weatherData);
                        }
                        SearchInProgress = false;
                        IsActivatedSearch = false;
                    }
                });
        }

        #region Commands
        public ICommand NavigateBack { get; }
        public ICommand GoToWeatherCommand { get; }
        public ICommand PerformSearch { private set; get; }
        #endregion

        #region Method
        private async Task<Location> GetLocation(string query)
        {
            IEnumerable<Xamarin.Essentials.Location> locations = new List<Xamarin.Essentials.Location>();
            try
            {
                locations = await Geocoding.GetLocationsAsync(query);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return locations?.FirstOrDefault();
        }
        public override void Init(object initData)
        {
            var existCurrentLocation = Preferences.ContainsKey("CurrentLocation");
            if (existCurrentLocation)
            {
                IsModalView = true;
            }
            else
            {
                IsModalView = false;
            }
        }
        #endregion

        #region Properties
        private bool _isActivatedSearch = false;
        public bool IsActivatedSearch
        {
            get { return _isActivatedSearch; }
            set => SetProperty(ref _isActivatedSearch, value);
        }

        private bool _isNotFoundLocation = false;
        public bool IsNotFoundLocation
        {
            get { return _isNotFoundLocation; }
            set => SetProperty(ref _isNotFoundLocation, value);
        }

        private bool _searchInProgress = false;
        public bool SearchInProgress
        {
            get { return _searchInProgress; }
            set => SetProperty(ref _searchInProgress, value);
        }
        #endregion
    }
}
