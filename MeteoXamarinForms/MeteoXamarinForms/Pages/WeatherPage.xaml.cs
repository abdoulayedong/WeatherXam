using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CoordinatorLayout.XamarinForms;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage
    {
        private bool _showLocation;

        public bool ShowLocation
        {
            get { return _showLocation; }
            set { OnPropertyChanged();
                  _showLocation = value; }
        }

        public WeatherPage()
        {
            InitializeComponent();
        }

        private void OnCoordinatorLayoutOnExpansionEventHandler(object sender, ExpansionEventArgs e)
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.Opacity = e.Progress;
            }

            if (MainTitle != null)
            {
                MainTitle.Opacity = e.Progress;
            }

            if (SecondaryTitle != null)
            {
                if (e.Progress > 0.2f)
                {
                    SecondaryTitle.Opacity = 0.0f;
                    LocationTitle.IsVisible = true;
                }
                else
                {
                    SecondaryTitle.Opacity = (0.2f - e.Progress) / 0.2f;
                    LocationTitle.IsVisible = false;
                }
            }
        }

        private void OnCoordinatorLayoutOnScrollEventHandler(object sender, ScrollEventArgs e)
        {
            if(MainTitle != null)
            {
                MainTitle.Opacity =  (0.2f - e.Progress) / 0.2f;
            }

            if(SecondaryTitle != null)
            {
                coordinator.ProportionalTopViewHeightMax = 0.2f - e.Progress;
            }
            //coordinator.ProportionalTopViewHeightMax = (0.2f - e.Progress) / 0.2f;


        }

        //protected override bool OnBackButtonPressed()
        //{
        //    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        //    return true;
        //}
    }
}