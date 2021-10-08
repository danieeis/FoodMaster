using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    [QueryProperty(nameof(FoodId), nameof(FoodId))]
    public class FoodDetailViewModel : BaseViewModel
    {
        private string foodId;
        private string text;
        private string description;
        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string FoodId
        {
            get
            {
                return foodId;
            }
            set
            {
                foodId = value;
            }
        }
    }
}
