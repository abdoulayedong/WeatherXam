using MeteoXamarinForms.Extensions;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using MeteoXamarinForms.ViewModels;
using Xamarin.Essentials;
using System.Threading.Tasks;
using FreshMvvm;
using MeteoXamarinForms.Services.Weather;
using MeteoXamarinForms.Services.Toast;
using MeteoXamarinForms.Resx;
using Xamarin.Forms.Internals;
using MeteoXamarinForms.Services;
using AutoMapper;
using System.Diagnostics;

namespace MeteoXamarinForms.PageModels
{
    public class CityPageModel : PageModelBase
    {
        public CityPageModel()
        {
            _weatherService = FreshIOC.Container.Resolve<IWeatherService>();
            CurrentTimezone = Preferences.Get("CurrentTimezone", "");
            _mapper = FreshIOC.Container.Resolve<IMapper>();

            #region Implemented Command
            AddWeatherInformationCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<SearchPageModel>(animate:true);
                });


            ShowWeatherInformationCommand = new Command<CityManager>(
                async (CityManager city) =>
                {
                    var selectedCity = CitiesManagerData.FirstOrDefault(data => data.Timezone.Contains(city.City));
                    Preferences.Set("CurrentTimezone", selectedCity.Timezone);
                    await CoreMethods.PushPageModel<WeatherPageModel>(data: selectedCity, animate:true);
                });

            DeleteWeatherInformationCommand = new Command<CityManager>(
                async (CityManager city) =>
                {
                    CitiesWeather.Remove(city);
                    var current = CitiesManagerData.FirstOrDefault(current => current.Timezone.Contains(city.City));
                    CitiesManagerData.Remove(current);
                    await SQLiteDataContext.Instance.DeleteAsync(current.Timezone);
                    if(current.Timezone == Preferences.Get("LocalTimezone", ""))
                    {
                        Preferences.Set("LocalTimezone", "");
                    }
                    if(current.Timezone == Preferences.Get("CurrentTimezone", ""))
                    {
                        Preferences.Set("CurrentTimezone", "");
                    }
                    if (CitiesManagerData.Count != 0 && Preferences.Get("CurrentTimezone", "") == "")
                    {
                        Preferences.Set("CurrentTimezone", CitiesManagerData.FirstOrDefault().Timezone);
                    }
                    if(CitiesManagerData.Count == 0) 
                    { 
                        CoreMethods.RemoveFromNavigation<WeatherPageModel>();
                        await CoreMethods.PushPageModel<SearchPageModel>(animate: true);
                        CoreMethods.RemoveFromNavigation<CityPageModel>();
                    }
                });

            ActualizeAllDataCommand = new Command(
                async () =>
                {
                    IsRefreshing = true;
                    await Update();
                    IsRefreshing = false;
                });
            #endregion
        }

        public override void Init(object initData)
        {
            Task.Run(async () => await Initialize()).Wait();
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
            if(Preferences.Get("CurrentTimezone", "") == CurrentTimezone)
            {
                await CoreMethods.PopPageModel(animate: true);
            }else if(Preferences.Get("CurrentTimezone", "") != CurrentTimezone)
            {
                string currentTimezone = Preferences.Get("CurrentTimezone", "");
                Root data = Task.Run(async () => await SQLiteDataContext.Instance.GetRootAsync(currentTimezone)).Result;
                await CoreMethods.PopPageModel(animate: true, data: data);
            }
        }
        #endregion
    }
}
