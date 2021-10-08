using FoodMaster.Services;
using FoodMaster.Views;
using System;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class RecipeHomeViewModel : BaseViewModel
    {

        public Command LogoutCommand { get; }
        UserService _userService;
        public RecipeHomeViewModel()
        {
            Title = "Home";
            _userService = DependencyService.Get<UserService>();
            LogoutCommand = new Command(Logout);
        }

        private void Logout(object obj)
        {
            _userService.InvalidateToken();
            App.Current.MainPage = new LoginPage();
        }
    }
}
