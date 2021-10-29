using MeteoXamarinForms.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityPage : ContentPage
    {
        public CityPage()
        {
            InitializeComponent();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    var vm = (CityPageModel)BindingContext;
        //    vm.BackPressCommand.Execute(true);
        //    return true;
        //}
    }
}