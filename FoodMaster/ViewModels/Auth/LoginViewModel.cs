using Acr.UserDialogs;
using FoodMaster.Services;
using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command LoginWithGoogle { get; }
        public Command LoginWithFacebook { get; }
        public Command GoRegister { get; }
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
        


        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            GoRegister = new Command(OnGoRegisterClicked);
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _userService = DependencyService.Get<UserService>();
#if DEBUG
            Email = "danieldaniyyelda@gmail.com";
            Password = "Aa.12345";
#endif
        }

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;
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

            string token = await _authenticationService.LoginWithEmailPassword(Email, Password);
            
            if (!string.IsNullOrEmpty(token))
            {
                await _userService.SaveToken(token).ConfigureAwait(false);
                if (_userService.PassThroughOnboarding)
                {
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    App.Current.MainPage = new OnboardingOne();
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Credenciales inválidas");
            }

            IsBusy = false;
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private void OnGoRegisterClicked(object obj)
        {
            App.Current.MainPage = new RegisterPage();
        }
    }
}
