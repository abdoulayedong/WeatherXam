using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CoordinatorLayout.XamarinForms;
using FormsControls.Base;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Medium, Subtype = AnimationSubtype.FromRight, Type = AnimationType.Push };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
        private bool _showLocation;

        public bool ShowLocation
        {
            get { return _showLocation; }
            set { OnPropertyChanged(); _showLocation = value; }
        }

        public WeatherPage()
        {
            InitializeComponent();
        }

        private void OnCoordinatorLayoutOnExpansionEventHandler(object sender, ExpansionEventArgs e)
        {    
            
            BackImage.Opacity = e.Progress;
            BackImageShadow.Opacity = e.Progress;
            MainTitle.Opacity = e.Progress;

            SecondaryTitle.Opacity = 1.0 - e.Progress;

            if(e.Progress == 0)
            {
                LocationTitle.IsVisible = false;
            }
            else
            {
                LocationTitle.IsVisible = true;
            }
        }
    }
}