using System;
using FoodMaster.Models;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    [QueryProperty(nameof(CategoryName), "CategoryName")]
    [QueryProperty(nameof(CategoryImage), "CategoryImage")]
    [QueryProperty(nameof(CategoryPath), "CategoryPath")]
    public class RecipeCategoryViewModel : BaseViewModel
    {
        public string CategoryImage { get; set; }
        public string CategoryName { get; set; }
        public string CategoryPath { get; set; }

        public RecipeCategoryViewModel()
        {
            
        }
    }
}
