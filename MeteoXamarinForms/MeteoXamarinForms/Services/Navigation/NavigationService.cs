using MeteoXamarinForms.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeteoXamarinForms.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public void CloseAsync()
        {
            System.Diagnostics.Process.GetCurrentProcess().Refresh();
        }

        public Task GoBackAsync()
        {
            return App.Current.MainPage.Navigation.PopAsync();
        }

        public async Task NavigateToAsync<TViewModelBase>(object navigationData = null, bool setRoute = false)
        {
            var view = ViewModelLocator.CreatePageFor(typeof(TViewModelBase));

            if (setRoute)
            {
                App.Current.MainPage = new NavigationPage(view);
            }
            else
            {
                if(App.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(view);
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(view);
                }
            }

            if(view.BindingContext is PageModelBase vmBase)
            {
                //await vmBase.InitializeAsync(navigationData);
            }
        }
    }
}
