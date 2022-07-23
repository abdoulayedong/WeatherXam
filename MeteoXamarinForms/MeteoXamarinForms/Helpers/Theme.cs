using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MeteoXamarinForms.Helpers
{
    public static class Theme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                case 0:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Unspecified;
                    break;
                case 1:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Light;
                    break;
                case 2:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Dark;
                    break;
            }

            var e = DependencyService.Get<IEnvironment>();
            if(App.Current.RequestedTheme == OSAppTheme.Dark)
            {
                string color =((Color)Application.Current.Resources["MainBackgroundColorDark"]).ToHex();
                e?.SetStatusBarColor(color, false);
            }
            else
            {
                string color = ((Color)Application.Current.Resources["MainBackgroundColor"]).ToHex();
                e?.SetStatusBarColor(color, true);
            }
        }
    }
}