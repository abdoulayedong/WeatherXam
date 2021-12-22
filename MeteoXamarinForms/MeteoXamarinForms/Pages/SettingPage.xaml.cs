﻿using FormsControls.Base;
using MeteoXamarinForms.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoXamarinForms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Medium, Subtype = AnimationSubtype.FromRight, Type = AnimationType.Push };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
    
        public SettingPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            var vm = (SettingPageModel)BindingContext;
            vm.BackPressCommand.Execute(true);
            return true;
        }
    }
}