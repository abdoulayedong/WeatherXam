using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Content.Res;

namespace MeteoXamarinForms.Droid
{
    [Activity(Label = "Meteo", Icon = "@mipmap/cloudy", Theme = "@style/LightTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Context GetContext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            GetContext = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FormsControls.Droid.Main.Init(this);
            ConfigurationChanged += MainActivity_ConfigurationChanged;
            UpdateThemeColor();
            LoadApplication(new App());
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        private void MainActivity_ConfigurationChanged(object sender, EventArgs e)
        {
            UpdateThemeColor();
            SetSearchBarColor();
        }

        private void UpdateThemeColor()
        {
            var config = GetContext.Resources.Configuration;
            var ThemeMode = config.UiMode == (UiMode.NightYes | UiMode.TypeNormal);
            if (ThemeMode)
                GetContext.SetTheme(Resource.Style.DarkTheme);
            else
                GetContext.SetTheme(Resource.Style.LightTheme);
        }

        private void SetSearchBarColor()
        {
            var result = int.Parse(Application.Resources.Configuration.UiMode.ToString());
            if (result == 33)
            {
                var icon = FindViewById(this.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
                (icon as ImageView)?.SetColorFilter(Color.White.ToAndroid());

            }
            else if (result == 17)
            {
                var icon = FindViewById(this.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
                (icon as ImageView)?.SetColorFilter(Android.Graphics.Color.Black);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}