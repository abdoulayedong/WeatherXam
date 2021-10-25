using FreshMvvm;
using MeteoXamarinForms.PageModels;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeteoXamarinForms.Navigation
{
    public class AnimatedNavigation : SharedTransitionNavigationPage, IFreshNavigationService
    {
        FreshTabbedNavigationContainer _tabbedNavigationPage;
        Page _cityPage;
        public AnimatedNavigation(Page page) : base(page) 
        {
            NavigationServiceName = "AnimatedNavigation";
            RegisterNavigation();
        }
        public async Task PopToRoot(bool animate = true)
        {
            await Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            throw new NotImplementedException();
        }

        public async Task PopPage(bool modal = false, bool animate = true)
        {
            if(modal)
                await Navigation.PopModalAsync(animate);
            else
                await Navigation.PopAsync(animate);
        }


        public async Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if(modal)
                await Navigation.PushModalAsync(page, animate);
            else
                await Navigation.PushAsync(page, animate);
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            IFreshNavigationService rootNavigation =
                FreshIOC.Container.Resolve<IFreshNavigationService>(NavigationServiceName);
            return Task.FromResult<FreshBasePageModel>(null);

        }

        protected void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public void LoadTabbedNav()
        {
            _tabbedNavigationPage = new FreshTabbedNavigationContainer();
            _cityPage = _tabbedNavigationPage.AddTab<CityPageModel>("Manage cities", "");
            //this.De = _tabbedNavigationPage;
        }
    }
}
