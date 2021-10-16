using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    [QueryProperty("CategoryId", "id")]
    public class RecipeCategoryViewModel : BaseViewModel, IQueryAttributable
    {
        string categoryImage;
        string categoryName;
        string categoryPath;
        string categoryType;

        public Command OpenFood { get; }

        public string CategoryImage
        {
            get => categoryImage;
            set => SetProperty(ref categoryImage, value);
        }
        public string CategoryName
        {
            get => categoryName;
            set => SetProperty(ref categoryName, value);
        }
        public string CategoryPath
        {
            get => categoryPath;
            set => SetProperty(ref categoryPath, value);
        }
        public string CategoryType
        {
            get => categoryType;
            set => SetProperty(ref categoryType, value);
        }

        ObservableCollection<Food> _foods;
        public ObservableCollection<Food> Foods
        {
            get => _foods;
            set => SetProperty(ref _foods, value);
        }


        IRecipeService _recipeService;

        public RecipeCategoryViewModel()
        {
            _recipeService = DependencyService.Get<IRecipeService>();
            OpenFood = new Command<Food>(OpenFoodAsync);
        }

        private async void OpenFoodAsync(Food food)
        {
            await Shell.Current.GoToAsync($"{nameof(FoodDetail)}?" +
                $"id={food.DocumentPath}");
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var id = HttpUtility.UrlDecode(query["id"]);
            var gastronomy = await _recipeService.GetGastronomy(id);
            CategoryName = gastronomy.Name;
            CategoryImage = gastronomy.Image;
            CategoryPath = gastronomy.DocumentPath;
            CategoryType = gastronomy.Type;
            Title = CategoryName;
            await GetAllFoods();

            _analyticsService.Track($"Tap: Categoria", new Dictionary<string, string>()
            {
                { "name", CategoryName },
                { "type", CategoryType }
            });
        }

        private async Task GetAllFoods()
        {
            IsBusy = true;
            var _foods = await _recipeService.GetFoods($"{CategoryPath}/platos");

            Foods = new ObservableCollection<Food>(_foods);
            IsBusy = false;
        }
    }
}
