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
        IAnalyticsService _analyticsService;
        


        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            GoRegister = new Command(OnGoRegisterClicked);
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _analyticsService = DependencyService.Get<IAnalyticsService>();
            _userService = DependencyService.Get<UserService>();
#if DEBUG
            Email = "danieldaniyyelda@gmail.com";
            Password = "Aa.12345";
#endif
        }

        private async void OnLoginClicked(object obj)
        {
            _analyticsService.Track("want login");
            IsBusy = true;
            if (string.IsNullOrEmpty(Email))
            {
                UserDialogs.Instance.Toast("Debe ingresar el email");
                _analyticsService.Track("Login: Validate email");
                IsBusy = false;
                return;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                UserDialogs.Instance.Toast("Debe ingresar la contraseña");
                _analyticsService.Track("Login: Validate password");
                IsBusy = false;
                return;
            }

            string token = await _authenticationService.LoginWithEmailPassword(Email, Password);
            
            if (!string.IsNullOrEmpty(token))
            {
                _analyticsService.Track("Login success");
                await _userService.SaveToken(token).ConfigureAwait(false);
                if (_userService.PassThroughOnboarding)
                {
                    _analyticsService.Track("Login to home directly");
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    _analyticsService.Track("Login to onboarding");
                    App.Current.MainPage = new OnboardingOne();
                }
            }
            else
            {
                _analyticsService.Track("Login failed");
                UserDialogs.Instance.Toast("Credenciales inválidas");
            }

            IsBusy = false;
        }

        private void OnGoRegisterClicked(object obj)
        {
            App.Current.MainPage = new RegisterPage();
        }
    }
}
