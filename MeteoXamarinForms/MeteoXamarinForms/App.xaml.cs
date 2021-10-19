using FreshMvvm;
using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using MeteoXamarinForms.Extensions;
using System;
using System.IO;

namespace MeteoXamarinForms
{
    public partial class App : Application
    {
        public App()
        {
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
                var splitFullFileName = fullFileName.Split('/');
                var fileName = splitFullFileName[splitFullFileName.Length - 1];
                var data = ToolExtension.GetDataLocaly(fileName.Split('.')[0]);
                var page = FreshPageModelResolver.ResolvePageModel<WeatherPageModel>(data);
                var navigationPage = new FreshNavigationContainer(page);
                MainPage = navigationPage;
            }
            else
            {
                var page = FreshPageModelResolver.ResolvePageModel<SearchPageModel>();
                var navigationPage = new FreshNavigationContainer(page);
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
