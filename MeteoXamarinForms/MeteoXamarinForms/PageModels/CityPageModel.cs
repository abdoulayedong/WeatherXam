﻿using MeteoXamarinForms.Extensions;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;
using MeteoXamarinForms.ViewModels;
using Xamarin.Essentials;
using System.Threading.Tasks;
using FreshMvvm;
using MeteoXamarinForms.Services.Weather;
using MeteoXamarinForms.Services.Toast;
using MeteoXamarinForms.Resx;

namespace MeteoXamarinForms.PageModels
{
    public class CityPageModel : PageModelBase
    {
        public CityPageModel()
        {
            _weatherService = FreshIOC.Container.Resolve<IWeatherService>();

            AddWeatherInformationCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<SearchPageModel>(animate:false);
                });


            ShowWeatherInformationCommand = new Command<CityManager>(
                async (CityManager city) =>
                {
                    Preferences.Set("FullFileName", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), String.Format("{0}.txt", city.City)));
                    var selectedCity = CitiesManagerData.Where(data => data.Timezone.Contains(city.City)).FirstOrDefault();
                    await CoreMethods.PushPageModel<WeatherPageModel>(data: selectedCity, animate:false);
                    CoreMethods.RemoveFromNavigation();
                });

            DeleteWeatherInformationCommand = new Command<CityManager>(
                async (CityManager city) =>
                {
                    CitiesWeather.Remove(city);
                    var current = CitiesManagerData.Where(current => current.Timezone.Contains(city.City)).FirstOrDefault();
                    CitiesManagerData.Remove(current);
                    ToolExtension.DeleteDataLocaly(current);
                    var fileList = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                    if (fileList.Count() == 0)
                    {
                        var existCurrentLocation = Preferences.ContainsKey("CurrentLocation");
                        if (existCurrentLocation)
                        {
                            Preferences.Remove("CurrentLocation");
                        }
                        await CoreMethods.PushPageModel<SearchPageModel>(animate: false);
                        CoreMethods.RemoveFromNavigation();
                    };
                });

            ActualizeAllDataCommand = new Command(
                async () =>
                {
                    IsRefreshing = true;
                    await Update();
                    IsRefreshing = false;
                });
        }

        public override void Init(object initData)
        {
            Initialize();
        }

        #region Methods
        private async Task Update()
        {
            foreach (var city in CitiesManagerData)
            {
                ToolExtension.DeleteDataLocaly(city);
                var localCityData = await _weatherService.GetWeatherFromLatLong(city.Lat, city.Lon);
                //localCityData.Timezone = city.Timezone;
                ToolExtension.SaveDataLocaly(localCityData, city.Timezone);
            }
            Initialize();
            DependencyService.Get<IToastService>().ShortToast(AppResources.UpdatedData);
        }

        private void Initialize()
        {
            CitiesManagerData = new List<Root>();
            CitiesWeather = new ObservableCollection<CityManager>();
            var files = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            foreach(var (file, index) in files.Select((value, i) => (value, i)))
            {
                CitiesManagerData.Add(ToolExtension.GetDataLocaly(file));
                var city = CitiesManagerData[index];
                var date = ToolExtension.GetDateTimeFromTimezone(city.Timezone_Offset);
                if (city.Timezone.Contains('/'))
                {
                    CitiesWeather.Add(new CityManager
                    {
                        City = ToolExtension.GetCityName(city.Timezone),
                        Temperature = ToolExtension.RoundedTemperature(city.Current.Temp),
                        Description = city.Current.Weather[0].Description,
                        Date = date
                    });
                }
                else
                {
                    CitiesWeather.Add(new CityManager
                    {
                        City = city.Timezone,
                        Temperature = ToolExtension.RoundedTemperature(city.Current.Temp),
                        Description = city.Current.Weather[0].Description, 
                        Date = date
                    });
                }
            }
        }
        #endregion

        #region Properties
        public Root Weather { get; set; }
        private readonly IWeatherService _weatherService;


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
            if (ToolExtension.ExistingWeatherData())
            {
                var fullFileName = Preferences.Get("FullFileName", String.Empty);
                if (fullFileName != String.Empty)
                {
                    var weatherData = ToolExtension.GetDataLocaly(fullFileName);
                    await CoreMethods.PushPageModel<WeatherPageModel>(animate: false, data: weatherData);
                }
                else
                {
                    var weatherData = ToolExtension.GetLastRegisterWeatherData();
                    await CoreMethods.PushPageModel<WeatherPageModel>(animate: false, data: weatherData);
                }
            }
        }
        #endregion
    }


    public class CityManager
    {    
        public string City { get; set; }
        public int Temperature { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
