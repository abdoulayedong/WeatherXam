using FormsControls.Base;
using System;
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

            searchBar.Focused += OnFocused;
            searchBar.Unfocused += OnUnfocused;
        }

        void OnUnfocused(object sender, FocusEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            searchBar.SetAppThemeColor(SearchBar.BackgroundColorProperty, Color.Transparent, Color.Transparent); 
        }

        void OnFocused(object sender, FocusEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            searchBar.SetAppThemeColor(SearchBar.BackgroundColorProperty,Color.Transparent, Color.Transparent);
        }
    }
}