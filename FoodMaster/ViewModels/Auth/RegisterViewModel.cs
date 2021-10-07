using System;
using Acr.UserDialogs;
using FoodMaster.Services;
using FoodMaster.Views;
using FoodMaster.Views.Onboarding;
using Xamarin.Forms;

namespace FoodMaster.ViewModels.Auth
{
    public class RegisterViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command RegisterWithGoogle { get; }
        public Command RegisterWithFacebook { get; }
        public Command GoLogin { get; }

        string names;
        public string Names
        {
            get => names;
            set => SetProperty(ref names, value);
        }
        string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        IAuthenticationService _authenticationService;


        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
            GoLogin = new Command(OnGoLoginClicked);
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        }

        private async void OnRegisterClicked(object obj)
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(Names))
            {
                UserDialogs.Instance.Toast("Debe ingresar los nombres");
                IsBusy = false;
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                UserDialogs.Instance.Toast("Debe ingresar el email");
                IsBusy = false;
                return;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                UserDialogs.Instance.Toast("Debe ingresar la contraseña");
                IsBusy = false;
                return;
            }

            string token = await _authenticationService.RegisterWithEmailPassword(Email, Password);
            if (!string.IsNullOrEmpty(token))
            {
                App.Current.MainPage = new OnboardingOne();
            }
            else
            {
                UserDialogs.Instance.Toast("Ocurrió un error al registrarse, intente nuevamente más tarde.");
            }

            IsBusy = false;
        }

        private void OnGoLoginClicked(object obj)
        {
            App.Current.MainPage = new LoginPage();

        }
    }
}
