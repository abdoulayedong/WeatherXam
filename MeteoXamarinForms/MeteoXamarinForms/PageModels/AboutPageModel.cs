using MeteoXamarinForms.ViewModels.Base;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MeteoXamarinForms.PageModels;

namespace MeteoXamarinForms.ViewModels
{
    public class AboutPageModel : PageModelBase
    {
        public AboutPageModel()
        {
        }

        #region Commands
        public ICommand BackPressCommand => new Command(BackPressMethod);
        #endregion

        #region Methods
        private async void BackPressMethod()
        {
            await CoreMethods.PushPageModel<SettingPageModel>(animate: false);
        }
        #endregion
    }
}
