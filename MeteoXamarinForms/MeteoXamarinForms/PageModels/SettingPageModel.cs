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

            Units = new ObservableCollection<Unit>()
            {
                new Unit{ Name = "°C", Parameter = "metric" },
                new Unit{ Name = "°F", Parameter = "imperial" },
                new Unit{ Name = "°K", Parameter = "standard" }
            };

            SelectedLanguage = SupportedLanguages.FirstOrDefault(lang => lang.CI == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
            SelectedUnit = Units.FirstOrDefault(unit => unit.Name == Preferences.Get("Unit", "°C"));

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
            set 
            { 
                _selectedLanguage = value;
                LocalizationResourceManager.Current.CurrentCulture = CultureInfo.GetCultureInfo(SelectedLanguage.CI);
                OnPropertyChanged("SelectedLanguage");
            }
        }

        private ObservableCollection<Unit> _units;

        public ObservableCollection<Unit> Units
        {
            get { return _units; }
            set => SetProperty(ref _units, value);
        }

        private Unit _selectedUnit;

        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                Preferences.Set("Unit", SelectedUnit.Name);
                Preferences.Set("UnitParameter", SelectedUnit.Parameter);
                OnPropertyChanged("SelectedUnit");
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
