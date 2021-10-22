using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;
using FoodMaster.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms.Xaml;

namespace FoodMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodDetail : ContentPage
    {
        PortionSelector PortionSelector;
        FoodDetailViewModel vm;
        public FoodDetail()
        {
            InitializeComponent();
            vm = BindingContext as FoodDetailViewModel;
            PortionSelector = new PortionSelector(vm);
        }


        async Task ReturnValuePopup()
        {
            await Navigation.PushPopupAsync(PortionSelector);

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
             await ReturnValuePopup();
        }
    }
}
