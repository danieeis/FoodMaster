using System;
using FoodMaster.Models;

namespace FoodMaster.ViewModels
{

    public class RecipeCategoryViewModel : BaseViewModel
    {
        public string CategoryImage { get; set; }
        public string CategoryName { get; set; }

        public RecipeCategoryViewModel()
        {
            
        }
    }
}
