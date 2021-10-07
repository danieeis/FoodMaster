using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FoodMaster.Services;
using FoodMaster.Views;
using FoodMaster.Views.Onboarding;

namespace FoodMaster
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<UserService>();
            var userService = DependencyService.Get<UserService>();
            if (userService.IsAuthenticated)
            {
                if (userService.PassThroughOnboarding)
                {
                    MainPage = new AppShell();
                }
                else
                {
                    MainPage = new OnboardingOne();
                }
                
            }
            else
            {
                MainPage = new LoginPage();
            }
            
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
