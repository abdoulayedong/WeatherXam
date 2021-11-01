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

        //private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
        //{

        //}

        //private void DropGestureRecognizer_Drop_Collection(object sender, DropEventArgs e)
        //{
        //    e.Handled = true;
        //}


        //protected override bool OnBackButtonPressed()
        //{
        //    var vm = (CityPageModel)BindingContext;
        //    vm.BackPressCommand.Execute(true);
        //    return true;
        //}
    }
}