using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                    gastronomy.Id = item.Id;
                    gastronomy.Order = int.Parse(item.GetString("order"));
                    gastronomy.Type = item.GetString("gastronomia");
                    gastronomy.Name = item.GetString("name");
                    gastronomy.Image = item.GetString("image");
                    gastronomy.DocumentPath = $"internacionales/{item.Id}";

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
                    gastronomy.Id = item.Id;
                    gastronomy.Order = int.Parse(item.GetString("order"));
                    gastronomy.Type = item.GetString("gastronomia");
                    gastronomy.Name = item.GetString("name");
                    gastronomy.Image = item.GetString("image");
                    gastronomy.DocumentPath = $"nacionales/{item.Id}";

                    gastronomies.Add(gastronomy);
                }


                return gastronomies;
            }

            return Enumerable.Empty<Gastronomy>();
        }

        public async Task<IEnumerable<Food>> GetFoods(string documentPath)
        {
            var foods = await FirebaseFirestore.Instance.Collection(documentPath).Get().ToAwaitableTask();

            List<Food> foodsList = new List<Food>();
            if (foods is QuerySnapshot cats)
            {
                foreach (DocumentSnapshot item in cats.Documents)
                {
                    Food food = new Food();
                    food.Id = item.Id;
                    food.Type = item.GetString("gastronomia");
                    food.Category = item.GetString("categoria");
                    food.Name = item.GetString("name");
                    food.Image = item.GetString("image");
                    food.Level = item.GetString("nivel_preparacion");
                    food.Timing = item.GetString("tiempo_preparacion");
                    //food.Tips = (string[]) item.Get("consejos");
                    //food.Preparation = (string[])item.Get("preparacion");
                    //food.NutritionalValue = (Dictionary<string,string>) item.Get("valor_nutrional").ToDictionary<string>();
                    //food.Ingredients = (Dictionary<string, string>) item.Get("ingredientes").ToDictionary<string>();
                    food.DocumentPath = $"{documentPath}/{item.Id}";

                    foodsList.Add(food);
                }


                return foodsList;
            }

            return Enumerable.Empty<Food>();
        }

        public async Task<Food> GetFood(string documentPath)
        {
            var foodDoc = await FirebaseFirestore.Instance.Document(documentPath).Get().ToAwaitableTask();

            if (foodDoc is DocumentSnapshot item)
            {
                Food food = new Food();
                food.Id = item.Id;
                food.Type = item.GetString("gastronomia");
                food.Category = item.GetString("categoria");
                food.Name = item.GetString("name");
                food.Image = item.GetString("image");
                food.Level = item.GetString("nivel_preparacion");
                food.Timing = item.GetString("tiempo_preparacion");
                //food.Tips = (string[]) item.Get("consejos");
                //food.Preparation = (string[])item.Get("preparacion");
                //food.NutritionalValue = (Dictionary<string,string>) item.Get("valor_nutrional").ToDictionary<string>();
                //food.Ingredients = (Dictionary<string, string>) item.Get("ingredientes").ToDictionary<string>();
                food.DocumentPath = $"{documentPath}";


                return food;
            }

            return new Food();
        }

        public async Task<Gastronomy> GetGastronomy(string documentPath)
        {
            var categories = await FirebaseFirestore.Instance.Document(documentPath).Get().ToAwaitableTask();

            if (categories is DocumentSnapshot item)
            {
                Gastronomy gastronomy = new Gastronomy();
                gastronomy.Id = item.Id;
                gastronomy.Type = item.GetString("gastronomia");
                gastronomy.Name = item.GetString("name");
                gastronomy.Image = item.GetString("image");
                gastronomy.DocumentPath = $"{documentPath}";


                return gastronomy;
            }

            return new Gastronomy();
        }
    }

    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null) ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                object value = property.GetValue(source);
                if (IsOfType<T>(value))
                {
                    dictionary.Add(property.Name, (T)value);
                }
            }
            return dictionary;
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new NullReferenceException("Unable to convert anonymous object to a dictionary. The source anonymous object is null.");
        }
    }
}
