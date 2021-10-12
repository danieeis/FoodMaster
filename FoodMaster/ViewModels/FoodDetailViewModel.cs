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
        IRecipeService _recipeService;
        public FoodDetailViewModel()
        {
            _recipeService = DependencyService.Get<IRecipeService>();
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var id = HttpUtility.UrlDecode(query["id"]);
            var food = await _recipeService.GetFood(id);
            Title = food.Name;
        }
    }
}
