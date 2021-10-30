using FreshMvvm;
using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using MeteoXamarinForms.Extensions;
using System;
using System.IO;
using Xamarin.CommunityToolkit.Helpers;
using MeteoXamarinForms.Resx;

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
                //var splitFullFileName = fullFileName.Split('/');
                //var fileName = splitFullFileName[splitFullFileName.Length - 1];
                Preferences.Set("FullFileName", fullFileName);
                var data = ToolExtension.GetDataLocaly(fullFileName);
                var page = FreshPageModelResolver.ResolvePageModel<WeatherPageModel>(data);
                //SharedTransitionNavigationPage navigationPage = new SharedTransitionNavigationPage(page);

                var navigationPage = new FreshNavigationContainer(page);
                navigationPage.BarBackground = Brush.Black;
                navigationPage.BarTextColor = Color.White;
                MainPage = navigationPage;

                //var customNavigationService = new AnimatedNavigation(page);
                //customNavigationService.BarBackground = Brush.Black;
                //customNavigationService.BarTextColor = Color.White;

                //FreshIOC.Container.Register<IFreshNavigationService>(customNavigationService);

                //MainPage = customNavigationService;
            }
            else
            {
                var page = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();
                var navigationPage = new FreshNavigationContainer(page);
                navigationPage.BarBackground = Brush.Black;
                navigationPage.BarTextColor = Color.White;
                MainPage = navigationPage;
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
