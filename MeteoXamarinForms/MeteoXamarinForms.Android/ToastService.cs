using Android.Widget;
using MeteoXamarinForms.Droid;
using MeteoXamarinForms.Services.Toast;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastService))]
namespace MeteoXamarinForms.Droid
{
    public class ToastService : IToastService
    {
        public void LongToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}