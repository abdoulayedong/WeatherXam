using FormsControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Medium, Subtype = AnimationSubtype.FromBottom, Type = AnimationType.Slide };

        public SearchPage()
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