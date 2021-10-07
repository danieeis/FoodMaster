using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FoodMaster.Services;
using FoodMaster.Views;

namespace FoodMaster
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
