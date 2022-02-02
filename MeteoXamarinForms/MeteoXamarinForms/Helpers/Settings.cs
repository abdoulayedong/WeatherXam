using Xamarin.Essentials;

namespace MeteoXamarinForms.Helpers
{
    /// <summary>
    /// Definition of color font the app theme
    /// 0 = default, 
    /// 1 = light, 
    /// 2 = dark
    /// </summary>
    public static class Settings
    {
        const int theme = 0;
        public static int Theme
        {
            get => Preferences.Get(nameof(Theme), theme);
            set => Preferences.Set(nameof(Theme), value);
        }
    }
}
