using System;
using System.Collections.Generic;
using FoodMaster.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingOne : ContentPage
    {
        public OnboardingOne()
        {
            InitializeComponent();
            this.BindingContext = new OnboardingViewModel();
        }
    }
}
