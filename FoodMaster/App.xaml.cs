using Xamarin.Forms;
using FoodMaster.Services;
using FoodMaster.Views;

namespace FoodMaster
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            RegisterServices();
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

        private static void RegisterServices()
        {
            DependencyService.Register<UserService>();
            DependencyService.Register<AnalyticsService>();
            DependencyService.Register<App>();
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
