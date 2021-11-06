using FreshMvvm;
using MeteoXamarinForms.Models;
using MeteoXamarinForms.Resx;
using MeteoXamarinForms.Services;
using MeteoXamarinForms.Services.Toast;
using MeteoXamarinForms.Services.Weather;
using MeteoXamarinForms.ViewModels;
using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
            _weatherService = FreshIOC.Container.Resolve<IWeatherService>();
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

            RefreshFrequencies = new()
            {
                new() { Name = new(() => AppResources.EveryHour), FrequencyTime = 1},
                new() { Name = new(() => AppResources.Every2Hours), FrequencyTime = 2},
                new() { Name = new(() => AppResources.Every6Hours), FrequencyTime = 6},
                new() { Name = new(() => AppResources.Every12Hours), FrequencyTime = 12},
                new() { Name = new(() => AppResources.Every24Hours), FrequencyTime = 24}
            };

            SelectedLanguage = SupportedLanguages.FirstOrDefault(lang => lang.CI == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
            SelectedUnit = Units.FirstOrDefault(unit => unit.Name == Preferences.Get("Unit", "°C"));
            SelectedFrequency = RefreshFrequencies.FirstOrDefault(frequency => frequency.FrequencyTime == Preferences.Get("FrequencyTime", 1));
            IsAutoRefresh = Preferences.Get("IsAutoRefresh", false);       


            ShowAboutPageCommand = new Command(
                async() =>
                {
                    await CoreMethods.PushPageModel<AboutPageModel>(animate: false);
                });

            TemperatureUnit = Preferences.Get("Unit", "°C");
            Language = SelectedLanguage;
        }

        #region Commands
        public ICommand ShowAboutPageCommand { private set; get; }
        public ICommand BackPressCommand => new Command(BackPressMethod);

        #endregion

        #region Properties
        public readonly string TemperatureUnit;
        public readonly Language Language;
        private readonly IWeatherService _weatherService;
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

        private bool _isAutoRefresh;
        public bool IsAutoRefresh
        {
            get { return _isAutoRefresh; }
            set 
            { 
                SetProperty(ref _isAutoRefresh, value);
                Preferences.Set("IsAutoRefresh", IsAutoRefresh);
            } 
        }

        private ObservableCollection<RefreshFrequency> _refreshFrequencies;
        public ObservableCollection<RefreshFrequency> RefreshFrequencies
        {
            get { return _refreshFrequencies; }
            set => SetProperty(ref _refreshFrequencies, value);
        }

        private RefreshFrequency _selectedFrequency;
        public RefreshFrequency SelectedFrequency
        {
            get { return _selectedFrequency; }
            set
            {
                _selectedFrequency = value;
                Preferences.Set("Frequency", SelectedFrequency.Name.Localized);
                Preferences.Set("FrequencyTime", SelectedFrequency.FrequencyTime);
                OnPropertyChanged("SelectedFrequency");
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
            if (TemperatureUnit == SelectedUnit.Name && Language == SelectedLanguage)
            {
                try
                {
                    Root data = await Task.Run(async () => await SQLiteDataContext.Instance.GetRootAsync(Preferences.Get("CurrentTimezone", "")));
                    data.Timezone = Preferences.Get("CurrentTimezone", "");
                    await CoreMethods.PushPageModel<WeatherPageModel>(data:data);
                }catch(Exception ex)
                {
                    DependencyService.Get<IToastService>().ShortToast(ex.Message);
                }
            }
            else 
            {
                string currentTimezone = Preferences.Get("CurrentTimezone", "");
                var lat = Preferences.Get("Lat", 6.1111);
                var lon = Preferences.Get("Long", 0.2222);
                Root data = await Task.Run(async () => await _weatherService.GetWeatherFromLatLong(lat, lon));
                data.Timezone = currentTimezone;
                await SQLiteDataContext.Instance.DeleteAsync(currentTimezone);
                data = await Task.Run(async () => await SQLiteDataContext.Instance.AddRoot(data));
                await CoreMethods.PushPageModel<WeatherPageModel>(animate: true, data: data);
            }

        }
        #endregion
    }
}
