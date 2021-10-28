using MeteoXamarinForms.Extensions;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.Resx;
using MeteoXamarinForms.ViewModels;
using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MeteoXamarinForms.PageModels
{
    public class SettingPageModel : PageModelBase
    {
        public SettingPageModel()
        {
            SupportedLanguages = new ObservableCollection<Language>()
            {
                new Language{Name = new (() => AppResources.English), CI = "en"},
                new Language{Name = new (() => AppResources.French), CI = "fr"}
            };

            SelectedLanguage = SupportedLanguages.FirstOrDefault(lang => lang.CI == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
            ShowAboutPageCommand = new Command(
                async() =>
                {
                    await CoreMethods.PushPageModel<AboutPageModel>(animate: false);
                });
        }

        #region Commands
        public ICommand ShowAboutPageCommand { private set; get; }
        public ICommand BackPressCommand => new Command(BackPressMethod);
        
        #endregion

        #region Properties
        private ObservableCollection<Language> _supportedLanguages;

        public ObservableCollection<Language> SupportedLanguages
        {
            get { return _supportedLanguages; }
            set => SetProperty(ref _supportedLanguages, value);
        }

        private Language _selectedLanguage;

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            //set => SetProperty(ref _selectedLanguage, value);
            set 
            { 
                _selectedLanguage = value;
                LocalizationResourceManager.Current.CurrentCulture = CultureInfo.GetCultureInfo(SelectedLanguage.CI);
                OnPropertyChanged("SelectedLanguage");
            }
        }

        #endregion

        #region Methods
        private async void BackPressMethod()
        {
            var fullFileName = Preferences.Get("FullFileName", String.Empty);
            if (fullFileName != String.Empty)
            {
                var weatherData = ToolExtension.GetDataLocaly(fullFileName);
                await CoreMethods.PushPageModel<WeatherPageModel>(animate: false, data: weatherData);
            }
        }
        #endregion
    }
}
