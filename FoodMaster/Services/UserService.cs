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
        User _user;
        IAuthenticationService _authenticationService;
        IAnalyticsService _analyticsService;
        public UserService()
        {
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            _analyticsService = DependencyService.Get<IAnalyticsService>();
            RetrieveUser();
        }

        public void RetrieveUser()
        {
            try
            {
                if (!IsAuthenticated) return;
                User = _authenticationService.GetUserAsync();
            }
            catch (Exception ex)
            {
                SignOut();
                _analyticsService.Report(ex);
            }
            
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

        public void InvalidateToken()
        {
            SecureStorage.Remove("OAuthToken");
            SignOut();
            IsAuthenticated = false;
            User = null;
        }

        private void SignOut()
        {
            _authenticationService.SignOut();

            App.Current.MainPage = new LoginPage();
        }
    }
}
