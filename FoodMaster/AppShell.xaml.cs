using FoodMaster.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodMaster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipeCategory), typeof(RecipeCategory));
            Routing.RegisterRoute(nameof(FoodDetail), typeof(FoodDetail));
            Routing.RegisterRoute(nameof(OnlineClassPage), typeof(OnlineClassPage));
        }
    }
}
