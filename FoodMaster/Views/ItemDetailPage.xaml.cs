using System.ComponentModel;
using Xamarin.Forms;
using FoodMaster.ViewModels;

namespace FoodMaster.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
