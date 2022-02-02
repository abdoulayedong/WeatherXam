using FormsControls.Base;
using Xamarin.Forms;

namespace MeteoXamarinForms.Pages
{
    public partial class AboutPage : ContentPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Medium, Subtype = AnimationSubtype.Default, Type = AnimationType.Fade };

        public void OnAnimationStarted(bool isPopAnimation)
        {
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
        }

        public AboutPage()
        {
            InitializeComponent();
        }
    }
}