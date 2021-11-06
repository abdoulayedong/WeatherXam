using FreshMvvm;
using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using MeteoXamarinForms.Extensions;
using System;
using System.IO;
using Xamarin.CommunityToolkit.Helpers;
using MeteoXamarinForms.Resx;
using MeteoXamarinForms.Services.Weather;
using AutoMapper;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.Services;
using System.Threading.Tasks;
using MeteoXamarinForms.Profiles;
using System.Collections.Generic;

namespace MeteoXamarinForms
{
    public partial class App : Application
    {
        public static IMapper CreateMapper()
        {
            List<Profile> profiles = new();
            profiles.Add(new CityManagerProfile());
            var configuration = new MapperConfiguration(conf =>
            {
                conf.AddProfiles(profiles);
            });
            return configuration.CreateMapper();
        }

        public App()
        {
            LocalizationResourceManager.Current.PropertyChanged += (_, _) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
            InitializeComponent();              
        }

        protected override void OnStart()
        {
            FreshIOC.Container.Register<IWeatherService, WeatherService>();
            IMapper mapper = App.CreateMapper();
            FreshIOC.Container.Register(mapper);

            //var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //var fileList = Directory.EnumerateFiles(folderPath);
            //DateTime lastDate = DateTime.MinValue;
            //string fullFileName = string.Empty;
            //foreach (var file in fileList)
            //{
            //    var creationTime = File.GetCreationTime(file);
            //    if (creationTime > lastDate)
            //    {
            //        fullFileName = file;
            //    }
            //}

            //if (fullFileName != string.Empty)
            //{
            //    Preferences.Set("FullFileName", fullFileName);
            //    var data = ToolExtension.GetDataLocaly(fullFileName);
            //    var page = FreshPageModelResolver.ResolvePageModel<WeatherPageModel>(data);

            //    var navigationPage = new FreshNavigationContainer(page)
            //    {
            //        BarBackground = Brush.Black,
            //        BarTextColor = Color.White
            //    };
            //    MainPage = navigationPage;
            //}
            //else
            //{
            //    var page = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();
            //    var navigationPage = new FreshNavigationContainer(page)
            //    {
            //        BarBackground = Brush.Black,
            //        BarTextColor = Color.White
            //    };
            //    MainPage = navigationPage;
            //}

            var currentTimezone = Preferences.Get("CurrentTimezone", "");
            if(currentTimezone == "")
            {
                Page page = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();
                FreshNavigationContainer navigationPage = new FreshNavigationContainer(page)
                {
                    BarBackground = Brush.Black,
                    BarTextColor = Color.White
                };
                MainPage = navigationPage;
            }
            else
            {
                Root data = Task.Run(async() => await SQLiteDataContext.Instance.GetRootAsync(currentTimezone)).Result;
                Page page = FreshPageModelResolver.ResolvePageModel<WeatherPageModel>(data);
                var navigationPage = new FreshNavigationContainer(page)
                {
                    BarBackground = Brush.Black,
                    BarTextColor = Color.White
                };
                MainPage = navigationPage;

            }

            if (!Preferences.ContainsKey("Unit"))
            {
                Preferences.Set("Unit", "°C");
                Preferences.Set("UnitParameter", "metric");
            }
        }

        //protected override void OnStart()
        //{
        //    FreshIOC.Container.Register<IWeatherService, WeatherService>();

        //    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    var fileList = Directory.EnumerateFiles(folderPath);
        //    DateTime lastDate = DateTime.MinValue;
        //    string fullFileName = string.Empty;
        //    foreach(var file in fileList)
        //    {
        //        var creationTime = File.GetCreationTime(file);
        //        if(creationTime > lastDate)
        //        {
        //            fullFileName = file;
        //        }
        //    }

        //    if (fullFileName != string.Empty)
        //    {              
        //        Preferences.Set("FullFileName", fullFileName);
        //        var data = ToolExtension.GetDataLocaly(fullFileName);
        //        var page = FreshPageModelResolver.ResolvePageModel<WeatherPageModel>(data);

        //        var navigationPage = new FreshNavigationContainer(page)
        //        {
        //            BarBackground = Brush.Black,
        //            BarTextColor = Color.White
        //        };
        //        MainPage = navigationPage;
        //    }
        //    else
        //    {
        //        var page = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();
        //        var navigationPage = new FreshNavigationContainer(page)
        //        {
        //            BarBackground = Brush.Black,
        //            BarTextColor = Color.White
        //        };
        //        MainPage = navigationPage;
        //    }

        //    if (!Preferences.ContainsKey("Unit"))
        //    {
        //        Preferences.Set("Unit", "°C");
        //        Preferences.Set("UnitParameter", "metric");
        //    }
        //}

    }
}
