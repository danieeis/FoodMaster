using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipeCategory), typeof(RecipeCategory));
            Routing.RegisterRoute(nameof(FoodDetail), typeof(FoodDetail));
        }
    }
}
