using System;
using System.Collections.Generic;
using FoodMaster.ViewModels;
using Xamarin.Forms;

namespace FoodMaster.Views.Onboarding
{
    public partial class OnboardingOne : ContentPage
    {
        public OnboardingOne()
        {
            InitializeComponent();
            this.BindingContext = new OnboardingViewModel();
        }
    }
}
