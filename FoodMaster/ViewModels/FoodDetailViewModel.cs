using System;
using System.Collections.Generic;
using System.Web;
using FoodMaster.Interfaces;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    [QueryProperty("FoodId", "id")]
    public class FoodDetailViewModel : BaseViewModel, IQueryAttributable
    {
        string _category;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        string _level;
        public string Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        string _timing;
        public string Timing
        {
            get => _timing;
            set => SetProperty(ref _timing, value);
        }

        string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        List<string> _tips;
        public List<string> Tips
        {
            get => _tips;
            set => SetProperty(ref _tips, value);
        }

        List<string> _preparation;
        public List<string> Preparation
        {
            get => _preparation;
            set => SetProperty(ref _preparation, value);
        }


        IRecipeService _recipeService;
        public FoodDetailViewModel()
        {
            _recipeService = DependencyService.Get<IRecipeService>();
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var id = HttpUtility.UrlDecode(query["id"]);
            var food = await _recipeService.GetFood(id);
            Name = Title = food.Name;
            Category = food.Category;
            Level = food.Level;
            Timing = food.Timing;
            Image = food.Image;
            Tips = food.Tips;
            Preparation = food.Preparation;
        }
    }
}
