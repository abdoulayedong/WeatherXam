using Xamarin.Forms;

namespace MeteoXamarinForms.Helpers
{
    public static class ThemeConfiguration
    {
        public static void SetTheme(string color, bool lightStatusBar)
        {
            var e = DependencyService.Get<IEnvironment>();
            e?.SetStatusBarColor(color, lightStatusBar);
        }
    }
}
