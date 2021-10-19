using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FoodMaster.Services;
using FoodMaster.Views;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
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
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IFacebookClient _facebookService = CrossFacebookClient.Current;

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
            RegisterWithGoogle = new Command(RegisterGoogleAsync);
            RegisterWithFacebook = new Command(async () => await RegisterWithFacebookAsync());
            GoLogin = new Command(OnGoLoginClicked);
            _userService = DependencyService.Get<UserService>();
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _analyticsService = DependencyService.Get<IAnalyticsService>();
        }

        async Task RegisterWithFacebookAsync()
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

                            _analyticsService.Track($"Facebook Register Success");
                            _userService.LoginMethod = "Facebook";
                            await _authenticationService.LoginWithFacebook(_facebookService.ActiveToken);
                            await LoginSucess(_facebookService.ActiveToken).ConfigureAwait(false);
                            break;
                        case FacebookActionStatus.Canceled:
                            _analyticsService.Track($"Facebook Register Cancel");
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

        async void RegisterGoogleAsync(object obj)
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
                            _analyticsService.Track($"Google Register Success");
                            _userService.LoginMethod = "Google";
                            await _authenticationService.LoginWithGoogle(_googleService.IdToken, _googleService.AccessToken);
                            await LoginSucess(_googleService.AccessToken).ConfigureAwait(false);
                            break;
                        case GoogleActionStatus.Canceled:
                            _analyticsService.Track($"Google Register Canceled");
                            UserDialogs.Instance.Toast("Cancelado al registrarse con Google");
                            break;
                        case GoogleActionStatus.Error:
                            _analyticsService.Track($"Google Register Error");
                            UserDialogs.Instance.Toast("Error al registrarse con Google");
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            _analyticsService.Track($"Google Register Unauthorized");
                            UserDialogs.Instance.Toast("No autorizado al registrarse con Google");
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
                UserDialogs.Instance.Toast("Ocurrió un error al registrarse con Google");
            }
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
                await LoginSucess(token).ConfigureAwait(false);
                _analyticsService.Track("Register with email success");
            }
            else
            {
                UserDialogs.Instance.Toast("Ocurrió un error al registrarse, intente nuevamente más tarde.");
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

        private void OnGoLoginClicked(object obj)
        {
            App.Current.MainPage = new LoginPage();

        }
    }
}
