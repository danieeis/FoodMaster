﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FoodMaster.Models;
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

        public UserService()
        {
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            User = _authenticationService.GetUserAsync();
        }

        public static event EventHandler<string> NameChanged
        {
            add => _nameChangedEventManager.AddEventHandler(value);
            remove => _nameChangedEventManager.RemoveEventHandler(value);
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

            if (token is null)
                throw new ArgumentNullException(nameof(token));



            await SecureStorage.SetAsync("OAuthToken", token).ConfigureAwait(false);
            User = _authenticationService.GetUserAsync();
            IsAuthenticated = true;
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
            _authenticationService.SignOut();
            IsAuthenticated = false;
            User = null;
        }

        void HandleLoggedOut(object sender, EventArgs e)
        {
            SecureStorage.Remove("OAuthToken");
            _authenticationService.SignOut();
            User = null;
            IsAuthenticated = false;
        }
    }
}
