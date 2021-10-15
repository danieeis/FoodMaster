using System;
using FoodMaster.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;

namespace FoodMaster.Views
{
    public partial class PortionSelector : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PortionSelector(FoodDetailViewModel foodDetail)
        {
            InitializeComponent();
            this.BindingContext = foodDetail;
        }
    }
}
