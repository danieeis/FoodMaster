using System;
using FoodMaster.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace FoodMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortionSelector : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PortionSelector(FoodDetailViewModel foodDetail)
        {
            InitializeComponent();
            this.BindingContext = foodDetail;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}
