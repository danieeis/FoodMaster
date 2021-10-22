using Xamarin.Forms;
using FoodMaster.Services;
using FoodMaster.Views;
using Xamarin.Forms.Xaml;

namespace FoodMaster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        UserService userService;
        public App()
        {
            InitializeComponent();
            userService = DependencyService.Get<UserService>();
            if (userService.IsAuthenticated)
            {
                DependencyService.Get<RemoteConfig>().Initialize().ConfigureAwait(false);
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
