using FormsControls.Base;
using Xamarin.Forms;

namespace MeteoXamarinForms.Pages
{

    public partial class SearchPage : ContentPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Medium, Subtype = AnimationSubtype.FromBottom, Type = AnimationType.Slide };

        public void OnAnimationStarted(bool isPopAnimation)
        {
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
        }

        public SearchPage()
        {
            InitializeComponent();
        }
    }
}