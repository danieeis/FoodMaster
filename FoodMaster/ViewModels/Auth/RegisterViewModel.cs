using System;
using Acr.UserDialogs;
using FoodMaster.Services;
using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
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
        UserService _userService;

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
            GoLogin = new Command(OnGoLoginClicked);
            _userService = DependencyService.Get<UserService>();
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _analyticsService = DependencyService.Get<IAnalyticsService>();
        }

        private async void OnRegisterClicked(object obj)
        {
            _analyticsService.Track("want register");
            IsBusy = true;
            if (string.IsNullOrEmpty(Names))
            {
                UserDialogs.Instance.Toast("Debe ingresar los nombres");
                _analyticsService.Track("Register: Validate names");
                IsBusy = false;
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                UserDialogs.Instance.Toast("Debe ingresar el email");
                _analyticsService.Track("Register: Validate email");
                IsBusy = false;
                return;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                UserDialogs.Instance.Toast("Debe ingresar la contraseña");
                _analyticsService.Track("Register: Validate password");
                IsBusy = false;
                return;
            }

            string token = await _authenticationService.RegisterWithEmailPassword(Names, Email, Password);
            if (!string.IsNullOrEmpty(token))
            {
                await _userService.SaveToken(token).ConfigureAwait(false);
                App.Current.MainPage = new OnboardingOne();
                _analyticsService.Track("Register with email success");
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
