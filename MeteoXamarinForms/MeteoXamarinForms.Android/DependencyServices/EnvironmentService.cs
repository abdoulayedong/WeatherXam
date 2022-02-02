using Android.OS;
using MeteoXamarinForms.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(MeteoXamarinForms.Droid.DependencyServices.Environment))]
namespace MeteoXamarinForms.Droid.DependencyServices
{
    public class Environment : IEnvironment
    {
        public void SetStatusBarColor(string color, bool lightStatusBar)
        {
            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                return;

            System.Drawing.Color toSetColor = Color.FromHex(color);
            var activity = Platform.CurrentActivity;
            var window = activity.Window;
            window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(toSetColor.ToPlatformColor());

            if(Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
                window.DecorView.SystemUiVisibility = lightStatusBar ? flag : 0;
            }
        }
    }

}