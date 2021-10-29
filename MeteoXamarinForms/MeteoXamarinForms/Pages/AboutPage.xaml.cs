using MeteoXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    var vm = (AboutPageModel)BindingContext;
        //    vm.BackPressCommand.Execute(true);
        //    return true;
        //}
    }
}