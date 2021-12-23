using Android.Content;
using MeteoXamarinForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace MeteoXamarinForms.Droid.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
           
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
            var plate = Control.FindViewById(plateId);
            plate.SetBackgroundColor(Android.Graphics.Color.Transparent);
            //var icon = Control?.FindViewById(Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
            //(icon as ImageView)?.SetColorFilter(Color.White.ToAndroid());
            //Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            //Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}