using System;
using System.Collections.Generic;
using FoodMaster.ViewModels;
using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
