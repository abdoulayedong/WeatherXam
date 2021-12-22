using FormsControls.Base;
using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Medium, Subtype = AnimationSubtype.Default, Type = AnimationType.Fade };

        public AboutPage()
        {
            InitializeComponent();
        }

        public void OnAnimationStarted(bool isPopAnimation)
        {
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
        }
    }
}