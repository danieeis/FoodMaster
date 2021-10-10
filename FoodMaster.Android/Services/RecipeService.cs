using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using FoodMaster.Droid.Services;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(RecipeService))]
namespace FoodMaster.Droid.Services
{
    public class RecipeService : IRecipeService
    {
        public async Task<IEnumerable<Gastronomy>> GetInternationalCategories()
        {
            var categories = await FirebaseFirestore.Instance.Collection("internacionales").Get().ToAwaitableTask();

            List<Gastronomy> gastronomies = new List<Gastronomy>();
            if (categories is QuerySnapshot cats)
            {
                foreach(DocumentSnapshot item in cats.Documents)
                {
                    Gastronomy gastronomy = new Gastronomy();
                    gastronomy.Name = item.GetString("name");
                    gastronomy.Image = item.GetString("image");

                    gastronomies.Add(gastronomy);
                }
                

                return gastronomies;
            }

            return Enumerable.Empty<Gastronomy>();
        }

        public async Task<IEnumerable<Gastronomy>> GetNationalCategories()
        {
            var categories = await FirebaseFirestore.Instance.Collection("nacionales").Get().ToAwaitableTask();

            List<Gastronomy> gastronomies = new List<Gastronomy>();
            if (categories is QuerySnapshot cats)
            {
                foreach (DocumentSnapshot item in cats.Documents)
                {
                    Gastronomy gastronomy = new Gastronomy();
                    gastronomy.Name = item.GetString("name");
                    gastronomy.Image = item.GetString("image");

                    gastronomies.Add(gastronomy);
                }


                return gastronomies;
            }

            return Enumerable.Empty<Gastronomy>();
        }
    }
}
