using FoodMaster.Interfaces;
using FoodMaster.Models;
using FoodMaster.Services;
using FoodMaster.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class RecipeHomeViewModel : BaseViewModel
    {

        public Command LogoutCommand { get; }
        public Command OpenCategory { get; }
        string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        ObservableCollection<Gastronomy> _nationalCategories;
        public ObservableCollection<Gastronomy> NationalCategories
        {
            get => _nationalCategories;
            set => SetProperty(ref _nationalCategories, value);
        }
        ObservableCollection<Gastronomy> _internationalCategories;
        public ObservableCollection<Gastronomy> InternationalCategories
        {
            get => _internationalCategories;
            set => SetProperty(ref _internationalCategories, value);
        }

        UserService _userService;
        IRecipeService _recipeService;
        public RecipeHomeViewModel()
        {
            Title = "Home";
            _userService = DependencyService.Get<UserService>();
            _recipeService = DependencyService.Get<IRecipeService>();
            Task.Run(GetAllCategories);
            LogoutCommand = new Command(Logout);
            OpenCategory = new Command<Gastronomy>(OpenGastronomy);
        }

        private void Logout(object obj)
        {
            _userService.InvalidateToken();
            App.Current.MainPage = new LoginPage();
        }

        private void OpenGastronomy(Gastronomy category)
        {

        }

        private async Task GetAllCategories()
        {
            IsBusy = true;
            var internationals = await _recipeService.GetInternationalCategories();
            var nationals = await _recipeService.GetNationalCategories();

            InternationalCategories = new ObservableCollection<Gastronomy>(internationals);
            NationalCategories = new ObservableCollection<Gastronomy>(nationals);
            IsBusy = false;
        }
    }
}
