using System;
using System.Linq;
using System.Threading.Tasks;
using FoodMaster.Models;
using FoodMaster.ViewModels;
using FoodMaster.Views;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodMaster.Services
{
    public class UserService
    {
        readonly static WeakEventManager<string> _nameChangedEventManager = new WeakEventManager<string>();

        User _user;
        IAuthenticationService _authenticationService;
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IFacebookClient _facebookService = CrossFacebookClient.Current;
        IAnalyticsService _analyticsService;
        public UserService()
        {
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _analyticsService = DependencyService.Get<IAnalyticsService>();
            RetrieveUser();
        }

        public async void RetrieveUser()
        {
            try
            {
                if (!IsAuthenticated) return;
                if (LoginMethod == "Email")
                {
                    User = _authenticationService.GetUserAsync();
                }
                else if (LoginMethod == "Google")
                {
                    var user = _googleService.CurrentUser;
                    User = new Models.User()
                    {
                        Email = user.Email,
                        Names = user.Name,
                        PhotoUrl = user.Picture?.AbsoluteUri,
                        Id = user.Id
                    };
                }else if(LoginMethod == "Facebook")
                {
                    await RetrieveFacebookData().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                SignOut();
                _analyticsService.Report(ex);
            }
            
        }

        async Task RetrieveFacebookData()
        {
            try
            {
                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;

                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:

                            _analyticsService.Track($"Facebook Retrieve Data Success");

                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            User = new User()
                            {
                                Email = facebookProfile.email,
                                Names = facebookProfile.name,
                                PhotoUrl = facebookProfile.picture.data.url,
                                Id = facebookProfile.id
                            };
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
            }
        }

        public static event EventHandler<string> NameChanged
        {
            add => _nameChangedEventManager.AddEventHandler(value);
            remove => _nameChangedEventManager.RemoveEventHandler(value);
        }

        public string LoginMethod
        {
            get => Preferences.Get(nameof(LoginMethod), "Email");
            set => Preferences.Set(nameof(LoginMethod), value);
        }

        public bool IsAuthenticated
        {
            get => Preferences.Get(nameof(IsAuthenticated), false);
            private set => Preferences.Set(nameof(IsAuthenticated), value);
        }

        public bool PassThroughOnboarding
        {
            get => Preferences.Get(nameof(PassThroughOnboarding), false);
            set => Preferences.Set(nameof(PassThroughOnboarding), value);
        }

        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                }
            }
        }

        public async Task SaveToken(string token)
        {
            IsAuthenticated = false;
            if (token is null)
                throw new ArgumentNullException(nameof(token));


            await SecureStorage.SetAsync("OAuthToken", token).ConfigureAwait(false);
            IsAuthenticated = true;
            RetrieveUser();
        }

        public async Task<string> GetAuthToken()
        {
            try
            {
                var token = await SecureStorage.GetAsync("OAuthToken").ConfigureAwait(false);

                if (token is null)
                    return string.Empty;

                IsAuthenticated = true;

                return token;
            }
            catch (ArgumentNullException)
            {
                IsAuthenticated = false;
                return string.Empty;
            }
        }

        public void InvalidateToken()
        {
            SecureStorage.Remove("OAuthToken");
            SignOut();
            IsAuthenticated = false;
            User = null;
        }

        private void SignOut()
        {
            if (LoginMethod == "Email")
            {
                _authenticationService.SignOut();
            }
            else if (LoginMethod == "Google")
            {
                _googleService.Logout();
            }else if(LoginMethod == "Facebook")
            {
                _facebookService.Logout();
            }


            App.Current.MainPage = new LoginPage();
        }

        void HandleLoggedOut(object sender, EventArgs e)
        {
            SecureStorage.Remove("OAuthToken");
            SignOut();
            User = null;
            IsAuthenticated = false;
        }
    }
}
