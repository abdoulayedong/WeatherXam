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
            try
            {
                _mapper = FreshIOC.Container.Resolve<IMapper>();
            }catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            #region Implemented Command
            AddWeatherInformationCommand = new Command(
                async () =>
                {
                    await CoreMethods.PushPageModel<SearchPageModel>(animate:true);
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

                    //CitiesWeather.Remove(city);
                    //var current = CitiesManagerData.Where(current => current.Timezone.Contains(city.City)).FirstOrDefault();
                    //CitiesManagerData.Remove(current);
                    //ToolExtension.DeleteDataLocaly(current);
                    //var fileList = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                    //if (fileList.Count() == 0)
                    //{
                    //    var existCurrentLocation = Preferences.ContainsKey("CurrentLocation");
                    //    if (existCurrentLocation)
                    //    {
                    //        Preferences.Remove("CurrentLocation");
                    //    }
                    //    await CoreMethods.PushPageModel<SearchPageModel>(animate: false);
                    //    CoreMethods.RemoveFromNavigation();
                    //};
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
            //List<Root> roots = new();
            //await SQLiteDataContext.Instance.DeleteRootsAsync();
            //foreach(var city in CitiesManagerData)
            //{
            //    Root cityData = await _weatherService.GetWeatherFromLatLong(city.Lat, city.Lon);
            //    cityData.Timezone = city.Timezone;
            //    Root root = await SQLiteDataContext.Instance.AddRoot(cityData);
            //    roots.Add(root);
            //};
            //CitiesManagerData = roots;
            //CitiesWeather = new();
            //CitiesManagerData.ForEach(city =>
            //{
            //    CitiesWeather.Add(_mapper.Map<CityManager>(city));
            //});
            //DependencyService.Get<IToastService>().ShortToast(AppResources.UpdatedData);

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
                CitiesWeather.Add(cityData);
            });
            DependencyService.Get<IToastService>().ShortToast(AppResources.UpdatedData);


            //foreach (var city in CitiesManagerData)
            //{
            //    ToolExtension.DeleteDataLocaly(city);
            //    var localCityData = await _weatherService.GetWeatherFromLatLong(city.Lat, city.Lon);
            //    localCityData.Timezone = city.Timezone;
            //    ToolExtension.SaveDataLocaly(localCityData, city.Timezone);
            //}
            //await Initialize();
            //await Initialize();
            //_mapper = FreshIOC.Container.Resolve<IMapper>();

            //foreach(var city in CitiesManagerData)
            //{
            //    //var cityData = _mapper.Map<CityManager>(city);
            //    //CitiesManagerData.Add(ToolExtension.GetDataLocaly(file));
            //    //var city = CitiesManagerData[index];
            //    var date = ToolExtension.GetDateTimeFromTimezone(city.Timezone_Offset);
            //    var placemarks = await Geocoding.GetPlacemarksAsync(city.Lat, city.Lon);
            //    var country = placemarks.FirstOrDefault();
            //    CitiesWeather.Add(
            //        new CityManager
            //        {
            //            City = ToolExtension.GetCityName(city.Timezone),
            //            Temperature = ToolExtension.RoundedTemperature(city.Current.Temp),
            //            Description = city.Current.Weather[0].Description,
            //            Date = date,
            //            Country = String.Format("{0}, {1}", country.AdminArea, country.CountryName),
            //            Icon = ToolExtension.GetIcon(city.Current.Weather[0].Icon)
            //        });
            //    }

            //CitiesManagerData.ForEach(city => CitiesWeather.Add(_mapper.Map<CityManager>(city)));
        }

        private async Task Initialize()
        {
            CitiesManagerData = new List<Root>();
            CitiesWeather = new ObservableCollection<CityManager>();
            List<Root> roots = await SQLiteDataContext.Instance.GetAllRoot();
            CitiesManagerData = roots;
            CitiesManagerData.ForEach(async (Root city) =>
            {
                var cityData = _mapper.Map<CityManager>(city);
                cityData.Country = await ToolExtension.GetCountry(city.Lat, city.Lon);
                cityData.IsLocalPosition = Preferences.Get("LocalTimezone", "") == city.Timezone ? true : false;
                CitiesWeather.Add(cityData);
            });
        }

        //private async Task Initialize()
        //{
        //    CitiesManagerData = new List<Root>();
        //    CitiesWeather = new ObservableCollection<CityManager>();
        //    var files = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        //    foreach(var (file, index) in files.Select((value, i) => (value, i)))
        //    {
        //        CitiesManagerData.Add(ToolExtension.GetDataLocaly(file));
        //        var city = CitiesManagerData[index];
        //        var date = ToolExtension.GetDateTimeFromTimezone(city.Timezone_Offset);
        //        var placemarks = await Geocoding.GetPlacemarksAsync(city.Lat, city.Lon);
        //        var country = placemarks.FirstOrDefault();
        //        if (city.Timezone.Contains('/'))
        //        {
        //            CitiesWeather.Add(new CityManager
        //            {
        //                City = ToolExtension.GetCityName(city.Timezone),
        //                Temperature = ToolExtension.RoundedTemperature(city.Current.Temp),
        //                Description = city.Current.Weather[0].Description,
        //                Date = date,
        //                Country = String.Format("{0}, {1}", country.AdminArea, country.CountryName),
        //                Icon = ToolExtension.GetIcon(city.Current.Weather[0].Icon)
        //            });
        //        }
        //        else
        //        {
        //            CitiesWeather.Add(new CityManager
        //            {
        //                City = city.Timezone,
        //                Temperature = ToolExtension.RoundedTemperature(city.Current.Temp),
        //                Description = city.Current.Weather[0].Description, 
        //                Date = date,
        //                Country = String.Format("{0}, {1}", country.AdminArea, country.CountryName),
        //                Icon = ToolExtension.GetIcon(city.Current.Weather[0].Icon)
        //            });
        //        }
        //    }
        //}

        //private void OnItemDragged(CityManager city)
        //{
        //    CitiesWeather.ForEach(c => c.IsBeingDragged = city == c);
        //}

        //private void OnItemDraggedOver(CityManager city)
        //{
        //    CityManager cityBeingDragged = _citiesWeather.FirstOrDefault(c => c.IsBeingDragged);
        //    CitiesWeather.ForEach(c => c.IsBeingDraggedOver = city == c && city != cityBeingDragged);
        //}

        //private void OnItemDragLeave(CityManager city)
        //{
        //    CitiesWeather.ForEach(c => c.IsBeingDraggedOver = false);
        //}

        //private async Task OnItemDropped(CityManager city)
        //{
        //    var cityToMove = _citiesWeather.First(i => i.IsBeingDragged);
        //    var cityToInsertBefore = city;

        //    if (cityToMove is null || cityToInsertBefore is null || cityToMove is null)
        //        return;

        //    var citiesWeatherToMoveFrom = CitiesWeather;
        //    citiesWeatherToMoveFrom.Remove(cityToMove);

        //    await Task.Delay(1000);

        //    var citiesWeatherToMoveTo = CitiesWeather;
        //    var insertAtIndex = _citiesWeather.IndexOf(cityToInsertBefore);

        //    citiesWeatherToMoveFrom.Insert(insertAtIndex, cityToMove);
        //    cityToMove.IsBeingDragged = false;
        //    cityToInsertBefore.IsBeingDraggedOver = false;
        //}
        #endregion

        #region Properties
        private readonly IWeatherService _weatherService;
        private IMapper _mapper;
        public readonly string CurrentTimezone;
        //public bool CurrentTimezoneDeleted { get; set; } = false;
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
            //if (ToolExtension.ExistingWeatherData())
            //{
            //    var fullFileName = Preferences.Get("FullFileName", String.Empty);
            //    if (fullFileName != String.Empty)
            //    {
            //        var weatherData = ToolExtension.GetDataLocaly(fullFileName);
            //        await CoreMethods.PushPageModel<WeatherPageModel>(animate: false, data: weatherData);
            //    }
            //    else
            //    {
            //        var weatherData = ToolExtension.GetLastRegisterWeatherData();
            //        await CoreMethods.PushPageModel<WeatherPageModel>(animate: false, data: weatherData);
            //    }
            //}
            if(Preferences.Get("CurrentTimezone", "") == CurrentTimezone)
            {
                await CoreMethods.PopPageModel(animate: true);
            }else if(Preferences.Get("CurrentTimezone", "") != CurrentTimezone)
            {
                string currentTimezone = Preferences.Get("CurrentTimezone", "");
                Root data = Task.Run(async () => await SQLiteDataContext.Instance.GetRootAsync(currentTimezone)).Result;
                await CoreMethods.PopPageModel(animate: true, data: data);
            }

            //if (CurrentTimezoneDeleted)
            //{
            //    //CoreMethods.RemoveFromNavigation<WeatherPageModel>(removeAll:true);
            //    await CoreMethods.PopPageModel(animate:true,data:CitiesManagerData.FirstOrDefault(timezone => timezone.Timezone == Preferences.Get("CurrentTimezone", "")));
            //}
            //else if(Preferences.Get("CurrentTimezone","") != "" && CurrentTimezoneDeleted)
            //{
            //    string currentTimezone = Preferences.Get("CurrentTimezone", "");
            //    Root data = Task.Run(async () => await SQLiteDataContext.Instance.GetRootAsync(currentTimezone)).Result;
            //    await CoreMethods.PopPageModel(animate: true, data:data);
            //}
            //{
            //    await CoreMethods.PopPageModel(animate: true);
            //}
        }
        #endregion
    }
}
