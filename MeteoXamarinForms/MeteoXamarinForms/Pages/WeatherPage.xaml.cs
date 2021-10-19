using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private void OnCoordinatorLayoutOnExpansionEventHandler(object sender, CoordinatorLayout.XamarinForms.ExpansionEventArgs e)
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

            if (SecondaryTitle is null)
            {

            }
        }

        private void OnCoordinatorLayoutOnScrollEventHandler(object sender, CoordinatorLayout.XamarinForms.ScrollEventArgs e)
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
                    ShowLocation = true;
                }
                else
                {
                    SecondaryTitle.Opacity = (0.2f - e.Progress) / 0.2f;
                    ShowLocation = false;
                }
            }

            if (SecondaryTitle is null)
            {

            }
        }
    }
}