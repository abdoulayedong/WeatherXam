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

namespace MeteoXamarinForms
{
    public partial class App : Application
    {
        public App()
        {

            LocalizationResourceManager.Current.PropertyChanged += (_, _) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
            InitializeComponent();              
        }

        protected override void OnStart()
        {
            FreshIOC.Container.Register<IWeatherService, WeatherService>();

            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fileList = Directory.EnumerateFiles(folderPath);
            DateTime lastDate = DateTime.MinValue;
            string fullFileName = string.Empty;
            foreach(var file in fileList)
            {
                var creationTime = File.GetCreationTime(file);
                if(creationTime > lastDate)
                {
                    fullFileName = file;
                }
            }
            
            if (fullFileName != string.Empty)
            {              
                Preferences.Set("FullFileName", fullFileName);
                var data = ToolExtension.GetDataLocaly(fullFileName);
                var page = FreshPageModelResolver.ResolvePageModel<WeatherPageModel>(data);

                var navigationPage = new FreshNavigationContainer(page)
                {
                    BarBackground = Brush.Black,
                    BarTextColor = Color.White
                };
                MainPage = navigationPage;
            }
            else
            {
                var page = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();
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

        protected override void OnSleep()
        { 
        }

        protected override void OnResume()
        {
        }
    }
}
