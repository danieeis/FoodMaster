using Acr.UserDialogs;
using FoodMaster.Services;
using FoodMaster.Views;
using FoodMaster.Views.Onboarding;
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
        bool IsLoading;


        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            GoRegister = new Command(OnGoRegisterClicked, (can) => !IsLoading);
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        }

        private async void OnLoginClicked(object obj)
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(Email))
            {
                UserDialogs.Instance.Toast("Debe ingresar el email");
                return;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                UserDialogs.Instance.Toast("Debe ingresar la contraseña");
                return;
            }

            string token = await _authenticationService.LoginWithEmailPassword(Email, Password);
            if (!string.IsNullOrEmpty(token))
            {
                App.Current.MainPage = new OnboardingOne();
            }
            else
            {
                UserDialogs.Instance.Toast("Credenciales inválidas");
            }
            IsLoading = false;

            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private void OnGoRegisterClicked(object obj)
        {
            App.Current.MainPage = new RegisterPage();
        }
    }
}
