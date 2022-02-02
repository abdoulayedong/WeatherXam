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
using System.Globalization;
using MeteoXamarinForms.Helpers;

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
            var existingCulture = CultureInfo.GetCultureInfo(Preferences.Get("Language", "en"));
            LocalizationResourceManager.Current.CurrentCulture = existingCulture;
            AppResources.Culture = existingCulture;
            LocalizationResourceManager.Current.PropertyChanged += (_, _) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
            InitializeComponent();              
        }

        protected override void OnStart()
        {
            Theme.SetTheme();
            FreshIOC.Container.Register<IWeatherService, WeatherService>();
            IMapper mapper = App.CreateMapper();
            FreshIOC.Container.Register(mapper);
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

            Application.Current.RequestedThemeChanged += (s, e) =>
            {
                Theme.SetTheme();
            };
        }

        protected override void OnSleep()
        {
            Theme.SetTheme();
            RequestedThemeChanged -= AppRequestedThemeChanged;
        }

        protected override void OnResume()
        {
            Theme.SetTheme();
            RequestedThemeChanged += AppRequestedThemeChanged;
        }

        private void AppRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Theme.SetTheme();
            });
        }
    }
}
