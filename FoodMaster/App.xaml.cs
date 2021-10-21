using Xamarin.Forms;
using FoodMaster.Services;
using FoodMaster.Views;

namespace FoodMaster
{
    public partial class App : Application
    {
        UserService userService;
        public App()
        {
            InitializeComponent();
            userService = DependencyService.Get<UserService>();
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

        

        protected override async void OnStart()
        {
            await DependencyService.Get<RemoteConfig>().Initialize().ConfigureAwait(false);            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
