using MeteoXamarinForms.Pages;
using MeteoXamarinForms.Services.Navigation;
using MeteoXamarinForms.Services.Weather;
using System;
using System.Collections.Generic;
using System.Text;
using TinyIoC;
using Xamarin.Forms;

namespace MeteoXamarinForms.ViewModels.Base
{
    public class ViewModelLocator
    {
        static TinyIoCContainer _container;
        static Dictionary<Type, Type> _viewLookup;

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();
            _viewLookup = new Dictionary<Type, Type>();

            // Register views and view models
            //Register<AboutPageModel, AboutPage>();
            //Register<WeatherPageModel, WeatherPage>();
            //Register<SearchPageModel, SearchPage>();

            // Register services 
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IWeatherService, WeatherService>();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        public static Page CreatePageFor(Type viewModelType)
        {
            var viewType = _viewLookup[viewModelType];
            var view = (Page)Activator.CreateInstance(viewType);
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
            return view;
        }

        static void Register<TViewModel, TView>() where TViewModel : PageModelBase where TView : Page
        {
            _viewLookup.Add(typeof(TViewModel), typeof(TView));
            _container.Register<TViewModel>();
        }
    }
}
