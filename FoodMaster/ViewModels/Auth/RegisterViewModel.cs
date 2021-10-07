using System;
using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster.ViewModels.Auth
{
    public class RegisterViewModel
    {
        public Command RegisterCommand { get; }
        public Command RegisterWithGoogle { get; }
        public Command RegisterWithFacebook { get; }
        public Command GoLogin { get; }


        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnLoginClicked);
            GoLogin = new Command(OnGoLoginClicked);
        }

        private void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private void OnGoLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            App.Current.MainPage = new LoginPage();

        }
    }
}
