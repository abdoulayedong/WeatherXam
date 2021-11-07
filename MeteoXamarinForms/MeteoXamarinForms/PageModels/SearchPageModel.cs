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
using MeteoXamarinForms.Models;
using System.Diagnostics;
using MeteoXamarinForms.Services.Toast;
using MeteoXamarinForms.Resx;
using MeteoXamarinForms.Services;

namespace MeteoXamarinForms.ViewModels
{
    public class SearchPageModel : PageModelBase
    {
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
                        Root weatherData;
                        try
                        {
                            weatherData = await _weatherService.GetWeatherFromLatLong(currentLocation.Latitude, currentLocation.Longitude);
                            Root data = await SQLiteDataContext.Instance.AddRoot(weatherData);
                            Preferences.Set("CurrentTimezone", weatherData.Timezone);
                            Preferences.Set("LocalTimezone", weatherData.Timezone);
                            await CoreMethods.PushPageModel<WeatherPageModel>(data: data);
                        }
                        catch (Exception ex)
                        {
                            DependencyService.Get<IToastService>().ShortToast(ex.Message);
                            Debug.WriteLine(ex.Message);
                        }                          
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
                        Root weatherData;
                        try
                        {
                            weatherData = await _weatherService.GetWeatherFromLatLong(location.Latitude, location.Longitude);
                            weatherData.Timezone = ToolExtension.GetTimezone(weatherData.Timezone, query);
                            Root data = await SQLiteDataContext.Instance.AddRoot(weatherData);
                            if(data.Id == 0)
                            {
                                DependencyService.Get<IToastService>().ShortToast("La position a déjà été ajoutée.");
                            }
                            else
                            {
                                Preferences.Set("CurrentTimezone", weatherData.Timezone);
                                await CoreMethods.PushPageModel<WeatherPageModel>(data: weatherData);
                            }
                        }
                        catch (Exception ex)
                        {
                            DependencyService.Get<IToastService>().ShortToast(ex.Message);
                            Debug.WriteLine(ex.Message);
                        }
                        SearchInProgress = false;
                        IsActivatedSearch = false;
                    }
                });

            VerifyLocalTimezone();
        }

        #region Commands
        public ICommand NavigateBack { get; }
        public ICommand GoToWeatherCommand { get; }
        public ICommand PerformSearch { private set; get; }
        #endregion

        #region Method
        private async Task<Location> GetLocation(string query)
        {
            IEnumerable<Location> locations = new List<Location>();
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

        private void VerifyLocalTimezone()
        {
            var existLocalTimezone = Preferences.Get("LocalTimezone", "");
            if (string.IsNullOrEmpty(existLocalTimezone) || string.IsNullOrWhiteSpace(existLocalTimezone))
            {
                ExistLocalTimezone = false;
            }
            else
            {
                ExistLocalTimezone = true;
            }
        }
        #endregion

        #region Properties
        private bool _isActivatedSearch = false;
        private readonly IWeatherService _weatherService;
        public bool ExistLocalTimezone { get; set; } 
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
