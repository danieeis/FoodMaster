using FoodMaster.Views;
using Xamarin.Forms;

namespace FoodMaster
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("categorydetail", typeof(RecipeCategory));
            Routing.RegisterRoute("fooddetail", typeof(FoodDetail));
        }
    }
}
