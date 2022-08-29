﻿using AutoMapper;
using FreshMvvm;
using MeteoXamarinForms.Extensions;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.Resx;
using MeteoXamarinForms.Services;
using MeteoXamarinForms.Services.Toast;
using MeteoXamarinForms.Services.Weather;
using MeteoXamarinForms.ViewModels;
using MeteoXamarinForms.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MeteoXamarinForms.PageModels
{
    public class CityPageModel : PageModelBase
    {
        public CityPageModel()
        {
            _weatherService = FreshIOC.Container.Resolve<IWeatherService>();
            CurrentTimezone = Preferences.Get("CurrentTimezone", "");
            _mapper = FreshIOC.Container.Resolve<IMapper>();
            var lang = Preferences.Get("Language", "en");
            switch (lang)
            {
                case "en":
                    AddCityButtonWidth = "en";
                    break;
                case "fr":
                    AddCityButtonWidth = "fr";
                    break;
                case "es":
                    AddCityButtonWidth = "es";
                    break;
                default:
                    AddCityButtonWidth = "en";
                    break;
            }
            #region Implemented Command
            AddWeatherInformationCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<SearchPageModel>();
                });


            ShowWeatherInformationCommand = new Command<CityManager>(
                async (CityManager city) =>
                {
                    var selectedCity = CitiesManagerData.FirstOrDefault(data => data.Timezone.Contains(city.City));
                    Preferences.Set("CurrentTimezone", selectedCity.Timezone);
                    await CoreMethods.PushPageModel<WeatherPageModel>(data: selectedCity);
                });

            DeleteWeatherInformationCommand = new Command<CityManager>(
                async (CityManager city) =>
                {
                    CitiesWeather.Remove(city);
                    var current = CitiesManagerData.FirstOrDefault(current => current.Timezone.Contains(city.City));
                    CitiesManagerData.Remove(current);
                    await SQLiteDataContext.Instance.DeleteAsync(current.Timezone);
                    if (current.Timezone == Preferences.Get("LocalTimezone", ""))
                    {
                        Preferences.Set("LocalTimezone", "");
                    }
                    if (current.Timezone == Preferences.Get("CurrentTimezone", ""))
                    {
                        Preferences.Set("CurrentTimezone", "");
                    }
                    if (CitiesManagerData.Count != 0 && Preferences.Get("CurrentTimezone", "") == "")
                    {
                        Preferences.Set("CurrentTimezone", CitiesManagerData.FirstOrDefault().Timezone);
                    }
                    if (CitiesManagerData.Count == 0)
                    {
                        CoreMethods.RemoveFromNavigation<WeatherPageModel>();
                        await CoreMethods.PushPageModel<SearchPageModel>();
                        CoreMethods.RemoveFromNavigation<CityPageModel>();
                    }
                });

            ActualizeAllDataCommand = new Command(
                async () =>
                {
                    IsRefreshing = true;
                    var current = Connectivity.NetworkAccess;
                    if(current == NetworkAccess.Internet)
                    {
                        await Update();
                    }
                    else
                    {
                        DependencyService.Get<IToastService>().ShortToast(AppResources.NoInternet);
                    }
                    IsRefreshing = false;
                });
            #endregion
        }

        public override void Init(object initData)
        {
            Task.Run(async () => await Initialize());
        }

        #region Methods
        private async Task Update()
        {
            List<Root> roots = new();
            await SQLiteDataContext.Instance.DeleteRootsAsync();
            foreach (var city in CitiesManagerData)
            {
                Root cityData = await _weatherService.GetWeatherFromLatLong(city.Lat, city.Lon);
                cityData.Timezone = city.Timezone;
                roots.Add(cityData);
            };
            CitiesManagerData.Clear();
            CitiesManagerData.AddRange(await SQLiteDataContext.Instance.AddRoots(roots));
            CitiesWeather = new();
            CitiesManagerData.ForEach(async (Root city) =>
            {
                var cityData = _mapper.Map<CityManager>(city);
                cityData.Country = await ToolExtension.GetCountry(city.Lat, city.Lon);
                cityData.IsLocalPosition = Preferences.Get("LocalTimezone", "") == city.Timezone ? true : false;
                CitiesWeather.Add(cityData);
            });
            DependencyService.Get<IToastService>().ShortToast(AppResources.UpdatedData);
        }

        private async Task Initialize()
        {
            dynamic result = await SQLiteDataContext.Instance.GetCityManager();
            if (result != null)
            {
                CitiesManagerData = result.roots;
                CitiesWeather = result.CitiesWeather;
            }
        }
        #endregion

        #region Properties
        private readonly IWeatherService _weatherService;
        private IMapper _mapper;
        public readonly string CurrentTimezone;
        private string _addCityButtonWidth;
        public string AddCityButtonWidth
        {
            get => _addCityButtonWidth;
            set => SetProperty(ref _addCityButtonWidth, value);
        }

        private ObservableCollection<CityManager> _citiesWeather;
        public ObservableCollection<CityManager> CitiesWeather
        {
            get => _citiesWeather;
            set => SetProperty(ref _citiesWeather, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set => SetProperty(ref _isRefreshing, value);
        }

        public List<Root> CitiesManagerData { get; set; }
        #endregion

        #region Commands
        public ICommand AddWeatherInformationCommand { private set; get; }
        public ICommand ShowWeatherInformationCommand { private set; get; }
        public ICommand DeleteWeatherInformationCommand { private set; get; }
        public ICommand ActualizeAllDataCommand { private set; get; }
        public ICommand BackPressCommand => new Command(BackPressMethod);
        #endregion

        #region Methods
        private async void BackPressMethod()
        {
            if (Preferences.Get("CurrentTimezone", "") == CurrentTimezone)
            {
                await CoreMethods.PopPageModel(animate: true);
            }
            else if (Preferences.Get("CurrentTimezone", "") != CurrentTimezone)
            {
                string currentTimezone = Preferences.Get("CurrentTimezone", "");
                Root data = Task.Run(async () => await SQLiteDataContext.Instance.GetRootAsync(currentTimezone)).Result;
                await CoreMethods.PopPageModel(animate: true, data: data);
            }
        }
        #endregion
    }
}
