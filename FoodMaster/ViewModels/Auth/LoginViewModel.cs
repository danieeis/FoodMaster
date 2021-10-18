using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FoodMaster.Services;
using FoodMaster.Views;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
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
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IFacebookClient _facebookService = CrossFacebookClient.Current;


        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            LoginWithGoogle = new Command(LoginGoogleAsync);
            LoginWithFacebook = new Command(async () => await LoginFacebookAsync());
            GoRegister = new Command(OnGoRegisterClicked);
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _userService = DependencyService.Get<UserService>();
#if DEBUG
            Email = "danieldaniyyelda@gmail.com";
            Password = "Aa.12345";
#endif
        }

        async Task LoginFacebookAsync()
        {
            try
            {

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;

                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:

                            _analyticsService.Track($"Facebook Login Success");
                            _userService.LoginMethod = "Facebook";
                            await LoginSucess(_facebookService.ActiveToken).ConfigureAwait(false);
                            break;
                        case FacebookActionStatus.Canceled:
                            _analyticsService.Track($"Facebook Login Cancel");
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "picture", "name" };
                string[] fbPermisions = { "email", "public_profile" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                _analyticsService.Report(ex);
                UserDialogs.Instance.Toast("Ocurrió un error al ingresar con Facebook");
            }
        }

        async void LoginGoogleAsync(object obj)
        {
            _analyticsService.Track("want login with google");
            try
            {
                if (!string.IsNullOrEmpty(_googleService.AccessToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
                            _analyticsService.Track($"Google Login Success");
                            _userService.LoginMethod = "Google";
                            await LoginSucess(_googleService.AccessToken).ConfigureAwait(false);
                            break;
                        case GoogleActionStatus.Canceled:
                            _analyticsService.Track($"Google Login Canceled");
                            UserDialogs.Instance.Toast("Cancelado al ingresar con Google");
                            break;
                        case GoogleActionStatus.Error:
                            _analyticsService.Track($"Google Login Error");
                            UserDialogs.Instance.Toast("Error al ingresar con Google");
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            _analyticsService.Track($"Google Login Unauthorized");
                            UserDialogs.Instance.Toast("No autorizado al ingresar con Google");
                            break;
                    }

                    _googleService.OnLogin -= userLoginDelegate;
                };

                _googleService.OnLogin += userLoginDelegate;

                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                _analyticsService.Report(ex);
                UserDialogs.Instance.Toast("Ocurrió un error al ingresar con Google");
            }
        }

        private async void OnLoginClicked(object obj)
        {
            _analyticsService.Track("want login with email");
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
                _userService.LoginMethod = "Email";
                await LoginSucess(token).ConfigureAwait(false);
            }
            else
            {
                _analyticsService.Track("Login failed");
                UserDialogs.Instance.Toast("Credenciales inválidas");
            }

            IsBusy = false;
        }

        private async Task LoginSucess(string token)
        {
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

        private void OnGoRegisterClicked(object obj)
        {
            App.Current.MainPage = new RegisterPage();
        }
    }
}
