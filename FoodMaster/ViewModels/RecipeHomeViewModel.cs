﻿using FoodMaster.Interfaces;
using FoodMaster.Models;
using FoodMaster.Services;
using FoodMaster.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class RecipeHomeViewModel : BaseViewModel
    {

        public Command LogoutCommand { get; }
        public Command OpenCategory { get; }
        public Command OpenOnlineClass { get; }
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
            OpenOnlineClass = new Command(GoOnlineClass);
        }

        private async void GoOnlineClass(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(OnlineClassPage)}");
        }

        private void Logout(object obj)
        {
            _userService.InvalidateToken();
        }

        private async void OpenGastronomy(Gastronomy category)
        {
            await Shell.Current.GoToAsync($"{nameof(RecipeCategory)}?" +
                $"id={category.DocumentPath}");
        }

        private async Task GetAllCategories()
        {
            IsBusy = true;
            var internationals = await _recipeService.GetInternationalCategories();
            var nationals = await _recipeService.GetNationalCategories();

            InternationalCategories = new ObservableCollection<Gastronomy>(internationals.OrderBy(x => x.Order));
            NationalCategories = new ObservableCollection<Gastronomy>(nationals.OrderBy(x => x.Order));
            IsBusy = false;
        }
    }
}
